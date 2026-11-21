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
using Frends.LinkedIn.SearchAdAnalytics.Definitions;
using Frends.LinkedIn.SearchAdAnalytics.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frends.LinkedIn.SearchAdAnalytics;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class LinkedIn
{
    private static readonly ObjectCache ClientCache = MemoryCache.Default;
    private static readonly CacheItemPolicy CachePolicy = new() { SlidingExpiration = TimeSpan.FromHours(1) };

    /// <summary>
    /// Gets or sets the HTTP client factory used to create clients.
    /// </summary>
    internal static IHttpClientFactory ClientFactory { get; set; } = new HttpClientFactory();

    /// <summary>
    /// Frends Task for fetching AdAnalytics from LinkedInAds.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.LinkedIn.SearchAdAnalytics).
    /// </summary>
    /// <param name="input">Input parameters.</param>
    /// <param name="options">Options parameters.</param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>Object { bool Success, object Body, Dictionary(string, string) Headers, int StatusCode, Error Error }.</returns>
    public static async Task<Result> SearchAdAnalytics([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        try
        {
            var uri = FormRequestURI(input);

            var client = GetHttpClientForOptions(options);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            using var responseMessage = await GetHttpRequestResponseAsync(
                    client,
                    uri,
                    cancellationToken)
                .ConfigureAwait(false);

            var responseBody = TryParseRequestStringResultAsJToken(await responseMessage.Content
                .ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false));

            var statusCode = (int)responseMessage.StatusCode;
            var errorMessage = $"LinkedIn returned HTTP status code {statusCode}.";
            if (!string.IsNullOrEmpty(options.ErrorMessageOnFailure))
            {
                errorMessage = $"{options.ErrorMessageOnFailure}: {errorMessage}";
            }

            return new Result
            {
                Success = responseMessage.IsSuccessStatusCode,
                Body = responseBody,
                Headers = GetResponseHeaderDictionary(responseMessage.Headers, responseMessage.Content.Headers),
                StatusCode = statusCode,
                Error = responseMessage.IsSuccessStatusCode
                    ? null
                    : new Error { Message = errorMessage },
            };
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

        using var request = new HttpRequestMessage(new HttpMethod("GET"), new Uri(url));

        HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (TaskCanceledException canceledException)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw;
            }

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
        {
            return httpClient;
        }

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
                + $":{options.AutomaticCookieHandling}";
    }

    private static string FormRequestURI(Input input)
    {
        var sb = new StringBuilder("https://api.linkedin.com/rest/adAnalytics");

        switch (input.Finder)
        {
            case Finder.Statistics:
                sb.Append("?q=statistics");
                if (input.Pivots.Any())
                {
                    sb.Append($"&pivots=List({string.Join(",", input.Pivots)})");
                }

                break;
            case Finder.Analytics:
                sb.Append("?q=analytics");
                if (input.Pivot != Pivot.NONE)
                {
                    sb.Append($"&pivot={input.Pivot}");
                }

                break;
        }

        sb.Append($"&timeGranularity={input.TimeGranularity}");

        if (DateTime.TryParse(input.StartDate, out var startDate))
        {
            sb.Append($"&dateRange=(start:(year:{startDate.Year},month:{startDate.Month},day:{startDate.Day})");
        }
        else
        {
            throw new ArgumentException("StartDate parameter was given in unsupported DateTime format.");
        }

        if (!string.IsNullOrEmpty(input.EndDate))
        {
            if (DateTime.TryParse(input.EndDate, out var endDate))
            {
                sb.Append($"&dateRange=(end:(year:{endDate.Year},month:{endDate.Month},day:{endDate.Day})");
            }
            else
            {
                throw new ArgumentException("EndDate parameter was given in unsupported DateTime format.");
            }
        }

        sb.Append(')');

        if (input.Campaigns.Any())
        {
            sb.Append($"&campaigns=List({WebUtility.UrlEncode(string.Join(",", input.Campaigns))})");
        }

        if (input.Creatives.Any())
        {
            sb.Append($"&creatives=List({WebUtility.UrlEncode(string.Join(",", input.Creatives))})");
        }

        if (input.Metrics.Any())
        {
            sb.Append($"&fields={string.Join(",", input.Metrics)}");
        }

        return sb.ToString();
    }
}
