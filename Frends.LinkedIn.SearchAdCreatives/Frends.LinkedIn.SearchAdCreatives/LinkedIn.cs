using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Frends.LinkedIn.SearchAdCreatives.Definitions;
using Frends.LinkedIn.SearchAdCreatives.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frends.LinkedIn.SearchAdCreatives;

/// <summary>
/// LinkedIn ad creative search task.
/// </summary>
public static class LinkedIn
{
    private static readonly ObjectCache ClientCache = MemoryCache.Default;
    private static readonly CacheItemPolicy CachePolicy = new() { SlidingExpiration = TimeSpan.FromHours(1) };

    internal static IHttpClientFactory ClientFactory { get; set; } = new HttpClientFactory();

    /// <summary>
    /// Searches for LinkedIn ad creatives.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.LinkedIn.SearchAdCreatives).
    /// </summary>
    /// <param name="input">Search criteria.</param>
    /// <param name="connection">LinkedIn connection and authentication parameters.</param>
    /// <param name="options">Error-handling options.</param>
    /// <param name="cancellationToken">Cancellation token provided by Frends.</param>
    /// <returns>object { bool Success, object Body, Dictionary(string, string) Headers, int StatusCode, object Error { string Message, Exception AdditionalInfo } }</returns>
    public static async Task<Result> SearchAdCreatives(
        [PropertyTab] Input input,
        [PropertyTab] Connection connection,
        [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            var uri = FormRequestURI(input);
            var client = GetHttpClientForConnection(connection, input.GetSingleCreative);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", connection.Token);

            using var responseMessage = await GetHttpRequestResponseAsync(client, uri, cancellationToken)
                .ConfigureAwait(false);
            var responseBody = TryParseRequestStringResultAsJToken(
                await responseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
            var responseHeaders = GetResponseHeaderDictionary(responseMessage.Headers, responseMessage.Content.Headers);
            var result = new Result
            {
                Success = true,
                Body = responseBody,
                Headers = responseHeaders,
                StatusCode = (int)responseMessage.StatusCode,
                Error = null,
            };

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new WebException(
                    $"Request to '{uri}' failed with status code {(int)responseMessage.StatusCode}. Response body: {result.Body}");
            }

            return result;
        }
        catch (Exception exception)
        {
            return exception.Handle(options);
        }
    }

    private static async Task<HttpResponseMessage> GetHttpRequestResponseAsync(
        HttpClient httpClient,
        string url,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
        try
        {
            return await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (TaskCanceledException canceledException)
        {
            if (cancellationToken.IsCancellationRequested)
                throw;

            throw new ArgumentException("HttpRequest was canceled, most likely due to a timeout.", canceledException);
        }
    }

    private static object TryParseRequestStringResultAsJToken(string response)
    {
        try
        {
            return string.IsNullOrWhiteSpace(response) ? new JValue(string.Empty) : JToken.Parse(response);
        }
        catch (JsonReaderException)
        {
            throw new JsonReaderException($"Unable to read response message as json: {response}");
        }
    }

    private static Dictionary<string, string> GetResponseHeaderDictionary(
        HttpResponseHeaders responseMessageHeaders,
        HttpContentHeaders contentHeaders)
    {
        var allHeaders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var header in contentHeaders)
            allHeaders[header.Key] = string.Join(";", header.Value);

        foreach (var header in responseMessageHeaders)
            allHeaders[header.Key] = string.Join(";", header.Value);

        return allHeaders;
    }

    private static HttpClient GetHttpClientForConnection(Connection connection, bool getSingleCreative)
    {
        var cacheKey = GetHttpClientCacheKey(connection);

        if (ClientCache.Get(cacheKey) is HttpClient cachedClient)
            return cachedClient;

        var httpClient = ClientFactory.CreateClient(connection);
        httpClient.SetDefaultRequestHeadersBasedOnConnection(connection, getSingleCreative);
        ClientCache.Add(cacheKey, httpClient, CachePolicy);
        return httpClient;
    }

    private static string GetHttpClientCacheKey(Connection connection)
    {
        return $"{connection.ConnectionTimeoutSeconds}"
               + $":{connection.FollowRedirects}:{connection.AllowInvalidCertificate}:{connection.AllowInvalidResponseContentTypeCharSet}"
               + $":{connection.AutomaticCookieHandling}:{connection.LinkedInAPIVersion}";
    }

    private static string FormRequestURI(Input input)
    {
        var builder = new StringBuilder($"https://api.linkedin.com/rest/adAccounts/{input.AdAccountId}/creatives");

        if (input.GetSingleCreative)
        {
            builder.Append($"/{HttpUtility.UrlEncode(input.AdCreativeUrn)}");
        }
        else
        {
            builder.Append('?');
            var campaigns = input.AdCampaignUrns?
                .Select(campaign => HttpUtility.UrlEncode(campaign.AdCampaignUrn))
                .ToList() ?? new List<string>();
            if (campaigns.Any())
                builder.Append($"campaigns=List({string.Join(",", campaigns)})&");

            var creatives = input.AdCreativesUrns?
                .Select(creative => HttpUtility.UrlEncode(creative.AdCreativeUrn))
                .ToList() ?? new List<string>();
            if (creatives.Any())
                builder.Append($"creatives=List({string.Join(",", creatives)})&");

            builder.Append("q=criteria");
        }

        return builder.ToString();
    }
}
