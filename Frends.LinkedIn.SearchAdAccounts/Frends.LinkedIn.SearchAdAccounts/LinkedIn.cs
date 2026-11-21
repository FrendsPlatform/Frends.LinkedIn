// <copyright file="LinkedIn.cs" company="Frends">
// Copyright (c) Frends. All rights reserved.
// </copyright>
global using System.ComponentModel;
global using System.Diagnostics.CodeAnalysis;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Runtime.Caching;
global using System.Runtime.ExceptionServices;
global using System.Text;
global using Frends.LinkedIn.SearchAdAccounts.Definitions;
global using Frends.LinkedIn.SearchAdAccounts.Helpers;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Linq;
global using LinkedInHttpClientFactory = Frends.LinkedIn.SearchAdAccounts.Definitions.IHttpClientFactory;

namespace Frends.LinkedIn.SearchAdAccounts;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class LinkedIn
{
    private static readonly ObjectCache ClientCache = MemoryCache.Default;
    private static readonly CacheItemPolicy CachePolicy = new() { SlidingExpiration = TimeSpan.FromHours(1) };

    /// <summary>
    /// Gets or sets the factory used to create clients for LinkedIn requests.
    /// </summary>
    internal static LinkedInHttpClientFactory ClientFactory { get; set; } = new HttpClientFactory();

    /// <summary>
    /// Searches LinkedIn for ad accounts matching the supplied criteria.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.LinkedIn.SearchAdAccounts).
    /// </summary>
    /// <param name="input">Search criteria.</param>
    /// <param name="options">Request and failure-handling options.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends.</param>
    /// <returns>object { bool Success, object Error { string Message, Exception AdditionalInfo }, dynamic Body, Dictionary(string, string) Headers, int StatusCode }.</returns>
    public static async Task<Result> SearchAdAccounts(
        [PropertyTab] Input input,
        [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            var uri = FormRequestURI(input);
            var client = GetHttpClientForOptions(options);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            using var responseMessage = await GetHttpRequestResponseAsync(client, uri, cancellationToken)
                .ConfigureAwait(false);
            var responseBody = TryParseRequestStringResultAsJToken(
                await responseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
            var result = new Result
            {
                Success = true,
                Body = responseBody,
                Headers = GetResponseHeaderDictionary(responseMessage.Headers, responseMessage.Content.Headers),
                StatusCode = (int)responseMessage.StatusCode,
                Error = null,
            };

            if (!responseMessage.IsSuccessStatusCode && options.ThrowExceptionOnErrorResponse)
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
            {
                throw;
            }

            throw new ArgumentException("HttpRequest was canceled, most likely due to a timeout.", canceledException);
        }
    }

    private static object TryParseRequestStringResultAsJToken(string response)
    {
        try
        {
            return string.IsNullOrWhiteSpace(response) ? new JValue(string.Empty) : JToken.Parse(response);
        }
        catch (JsonReaderException exception)
        {
            throw new JsonReaderException($"Unable to read response message as json: {response}", exception);
        }
    }

    private static Dictionary<string, string> GetResponseHeaderDictionary(
        HttpResponseHeaders responseMessageHeaders,
        HttpContentHeaders contentHeaders)
    {
        var responseHeaders = responseMessageHeaders.ToDictionary(header => header.Key, header => string.Join(";", header.Value));
        var allHeaders = contentHeaders.ToDictionary(header => header.Key, header => string.Join(";", header.Value));
        foreach (var header in responseHeaders)
        {
            allHeaders[header.Key] = header.Value;
        }

        return allHeaders;
    }

    private static HttpClient GetHttpClientForOptions(Options options)
    {
        var cacheKey = GetHttpClientCacheKey(options);
        if (ClientCache.Get(cacheKey) is HttpClient cachedClient)
        {
            return cachedClient;
        }

        var httpClient = ClientFactory.CreateClient(options);
        httpClient.SetDefaultRequestHeadersBasedOnOptions(options);
        ClientCache.Add(cacheKey, httpClient, CachePolicy);
        return httpClient;
    }

    private static string GetHttpClientCacheKey(Options options)
    {
        return $"{options.ConnectionTimeoutSeconds}"
            + $":{options.FollowRedirects}:{options.AllowInvalidCertificate}:{options.AllowInvalidResponseContentTypeCharSet}"
            + $":{options.ThrowExceptionOnErrorResponse}:{options.AutomaticCookieHandling}";
    }

    private static string FormRequestURI(Input input)
    {
        var builder = new StringBuilder("https://api.linkedin.com/rest/adAccounts?q=search&search=(");
        var filters = new Dictionary<string, List<string>>();

        if (!string.IsNullOrEmpty(input.Id))
        {
            filters["id"] = new List<string> { input.Id };
        }

        if (!string.IsNullOrEmpty(input.Name))
        {
            filters["name"] = new List<string> { input.Name };
        }

        if (!string.IsNullOrEmpty(input.Reference))
        {
            filters["reference"] = new List<string> { input.Reference };
        }

        var typeFilters = input.TypeFilters?.Select(filter => filter.AccountType.ToString()).ToList() ?? new List<string>();
        if (typeFilters.Any())
        {
            filters["type"] = typeFilters;
        }

        var statusFilters = input.StatusFilters?.Select(filter => filter.AccountStatus.ToString()).ToList() ?? new List<string>();
        if (statusFilters.Any())
        {
            filters["status"] = statusFilters;
        }

        foreach (var filter in filters)
        {
            builder.Append($"{filter.Key}:(values:List({string.Join(",", filter.Value)})),");
        }

        if (filters.Count > 0)
        {
            builder.Length--;
        }

        return builder.Append(')').ToString();
    }
}
