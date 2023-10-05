namespace Frends.LinkedIn.SearchAdCampaigns;

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using Frends.LinkedIn.SearchAdCampaigns.Definitions;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class LinkedIn
{
    private static readonly IHttpClientFactory ClientFactory = new HttpClientFactory();
    private static readonly ObjectCache ClientCache = MemoryCache.Default;
    private static readonly CacheItemPolicy CachePolicy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromHours(1) };

    /// <summary>
    /// Frends Task for finding LinkedIn Campaigns using LinkedIn Rest API.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.LinkedIn.SearchAdCampaigns).
    /// </summary>
    /// <param name="filter">Filter parameters.</param>
    /// <param name="options">Options parameters.</param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>Object { string Output }.</returns>
    public static async Task<Result> SearchAdCampaigns([PropertyTab] Filter filter, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        var uri = FormRequestURI(filter);

        var client = GetHttpClientForOptions(options);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

        using var responseMessage = await GetHttpRequestResponseAsync(
                client,
                uri,
                cancellationToken)
            .ConfigureAwait(false);

        dynamic response;

        var rbody = TryParseRequestStringResultAsJToken(await responseMessage.Content
            .ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false));
        var rstatusCode = (int)responseMessage.StatusCode;
        var rheaders = GetResponseHeaderDictionary(responseMessage.Headers, responseMessage.Content.Headers);
        response = new Result(rbody, rheaders, rstatusCode);

        if (!responseMessage.IsSuccessStatusCode && options.ThrowExceptionOnErrorResponse)
        {
            throw new WebException(
                $"Request to '{uri}' failed with status code {(int)responseMessage.StatusCode}. Response body: {response.Body}");
        }

        return response;
    }

    private static async Task<HttpResponseMessage> GetHttpRequestResponseAsync(
            HttpClient httpClient,
            string url,
            CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var request = new HttpRequestMessage(new HttpMethod("GET"), new Uri(url));

        HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (TaskCanceledException canceledException)
        {
            if (cancellationToken.IsCancellationRequested)
                throw;

            // Cancellation is from inside of the request, mostly likely a timeout
            throw new ArgumentException("HttpRequest was canceled, most likely due to a timeout.", canceledException);
        }

        return response;
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

    private static Dictionary<string, string> GetResponseHeaderDictionary(HttpResponseHeaders responseMessageHeaders, HttpContentHeaders contentHeaders)
    {
        var responseHeaders = responseMessageHeaders.ToDictionary(h => h.Key, h => string.Join(";", h.Value));
        var allHeaders = contentHeaders?.ToDictionary(h => h.Key, h => string.Join(";", h.Value)) ?? new Dictionary<string, string>();
        responseHeaders.ToList().ForEach(x => allHeaders[x.Key] = x.Value);
        return allHeaders;
    }

    private static HttpClient GetHttpClientForOptions(Options options)
    {
        var cacheKey = GetHttpClientCacheKey(options);

        if (ClientCache.Get(cacheKey) is HttpClient httpClient)
            return httpClient;

        httpClient = ClientFactory.CreateClient(options);
        httpClient.SetDefaultRequestHeadersBasedOnOptions(options);

        ClientCache.Add(cacheKey, httpClient, CachePolicy);

        return httpClient;
    }

    private static string GetHttpClientCacheKey(Options options)
    {
        // Includes everything except for options.Token, which is used on request level, not http client level
        return $"{options.ConnectionTimeoutSeconds}"
               + $":{options.FollowRedirects}:{options.AllowInvalidCertificate}:{options.AllowInvalidResponseContentTypeCharSet}"
               + $":{options.ThrowExceptionOnErrorResponse}:{options.AutomaticCookieHandling}";
    }

    private static string FormRequestURI(Filter options)
    {
        var sb = new StringBuilder($"https://api.linkedin.com/rest/adAccounts/{options.AdAccountId}/adCampaigns");

        sb.Append("?q=search&search=(");

        var dict = new Dictionary<string, List<string>>();

        if (!string.IsNullOrEmpty(options.Id))
            dict["id"] = new List<string> { options.Id };

        if (!string.IsNullOrEmpty(options.Name))
            dict["name"] = new List<string> { options.Name };

        if (!string.IsNullOrEmpty(options.Reference))
            dict["reference"] = new List<string> { options.Reference };

        var typeFilters = options.TypeFilters != null ? options.TypeFilters.Select(x => x.CampaignType.ToString()).ToList() : new List<string>();
        if (typeFilters.Any())
            dict["type"] = typeFilters;

        var statusFilters = options.StatusFilters != null ? options.StatusFilters.Select(x => x.CampaignStatus.ToString()).ToList() : new List<string>();
        if (statusFilters.Any())
            dict["status"] = statusFilters;

        foreach (var obj in dict)
            sb.Append($"{obj.Key}:(values:List({string.Join(",", obj.Value)})),");

        if (sb.ToString().EndsWith(','))
            sb.Remove(sb.Length - 1, 1);

        sb.Append(')');

        return sb.ToString();
    }
}
