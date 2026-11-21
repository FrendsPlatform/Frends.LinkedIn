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
using Frends.LinkedIn.SearchAdAccountUser.Definitions;
using Frends.LinkedIn.SearchAdAccountUser.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frends.LinkedIn.SearchAdAccountUser;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class LinkedIn
{
    private static readonly ObjectCache ClientCache = MemoryCache.Default;
    private static readonly CacheItemPolicy CachePolicy = new() { SlidingExpiration = TimeSpan.FromHours(1) };

    internal static IHttpClientFactory ClientFactory { get; set; } = new HttpClientFactory();

    /// <summary>
    /// Frends Task for searching a user from LinkedIn Ad Account.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.LinkedIn.SearchAdAccountUser).
    /// </summary>
    /// <param name="input">Search parameters.</param>
    /// <param name="connection">LinkedIn connection parameters.</param>
    /// <param name="options">Additional task options.</param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>object { bool Success, object Body, Dictionary(string, string) Headers, int StatusCode, object Error }</returns>
    public static async Task<Result> SearchAdAccountUser(
        [PropertyTab] Input input,
        [PropertyTab] Connection connection,
        [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        string uri;
        Result result;
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            uri = FormRequestURI(input);
            var client = GetHttpClientForOptions(options);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", connection.Token);

            using var responseMessage = await GetHttpRequestResponseAsync(client, uri, cancellationToken)
                .ConfigureAwait(false);
            var body = TryParseRequestStringResultAsJToken(
                await responseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
            var statusCode = (int)responseMessage.StatusCode;
            var headers = GetResponseHeaderDictionary(responseMessage.Headers, responseMessage.Content.Headers);
            result = new Result
            {
                Success = responseMessage.IsSuccessStatusCode,
                Body = body,
                Headers = headers,
                StatusCode = statusCode,
            };
        }
        catch (Exception exception)
        {
            return exception.Handle(options);
        }

        if (!result.Success)
        {
            var exception = new WebException(
                $"Request to '{uri}' failed with status code {result.StatusCode}. Response body: {result.Body}");
            var failure = exception.Handle(options);
            failure.Body = result.Body;
            failure.Headers = result.Headers;
            failure.StatusCode = result.StatusCode;
            return failure;
        }

        return result;
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
        var responseHeaders = responseMessageHeaders.ToDictionary(header => header.Key, header => string.Join(";", header.Value));
        var allHeaders = contentHeaders?.ToDictionary(header => header.Key, header => string.Join(";", header.Value))
            ?? new Dictionary<string, string>();
        responseHeaders.ToList().ForEach(header => allHeaders[header.Key] = header.Value);
        return allHeaders;
    }

    private static HttpClient GetHttpClientForOptions(Options options)
    {
        var cacheKey = GetHttpClientCacheKey(options);

        if (ClientCache.Get(cacheKey) is HttpClient cachedClient)
            return cachedClient;

        var httpClient = ClientFactory.CreateClient(options);
        httpClient.SetDefaultRequestHeadersBasedOnOptions(options);
        ClientCache.Add(cacheKey, httpClient, CachePolicy);
        return httpClient;
    }

    private static string GetHttpClientCacheKey(Options options)
    {
        return $"{options.ConnectionTimeoutSeconds}"
               + $":{options.FollowRedirects}:{options.AllowInvalidCertificate}:{options.AllowInvalidResponseContentTypeCharSet}"
               + $":{options.AutomaticCookieHandling}";
    }

    private static string FormRequestURI(Input input)
    {
        var builder = new StringBuilder("https://api.linkedin.com/rest/adAccountUsers");

        switch (input.SearchMethod)
        {
            case SearchAttribute.GetAdAccountUser:
                if (string.IsNullOrEmpty(input.AccountUrn) || string.IsNullOrEmpty(input.UserUrn))
                    throw new ArgumentException($"Search method {input.SearchMethod} requires both AccountUrn and UserUrn.");
                builder.Append($"/account={input.AccountUrn}&user={input.UserUrn}");
                return builder.ToString();
            case SearchAttribute.FindAdAccountsByAuthenticatedUser:
                builder.Append("?q=authenticatedUser");
                return builder.ToString();
            case SearchAttribute.FindAdAccountUsersByAccounts:
                if (input.AccountUrns is null || input.AccountUrns.Length == 0)
                    throw new ArgumentException($"Search method {input.SearchMethod} needs at least one AccountUrn.");
                builder.Append("?q=accounts");
                foreach (var account in input.AccountUrns)
                {
                    if (string.IsNullOrEmpty(account?.Urn))
                        throw new ArgumentException("Each AccountUrn must include a URN.");
                    builder.Append($"&accounts={account.Urn}");
                }

                return builder.ToString();
            default:
                throw new ArgumentOutOfRangeException(nameof(input.SearchMethod), input.SearchMethod, "Unknown search method.");
        }
    }
}
