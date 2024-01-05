namespace Frends.LinkedIn.Request
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.Caching;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Frends.LinkedIn.Request.Definitions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Main class of the Task.
    /// </summary>
    public static class LinkedIn
    {
        private static readonly IHttpClientFactory ClientFactory = new HttpClientFactory();
        private static readonly ObjectCache ClientCache = MemoryCache.Default;
        private static readonly CacheItemPolicy CachePolicy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromHours(1) };

        /// <summary>
        /// Frends Task for sending Http request to LinkedIn API.
        /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.LinkedIn.Request).
        /// </summary>
        /// <param name="input">Input parameters.</param>
        /// <param name="options">Options parameters.</param>
        /// <param name="cancellationToken">Cancellation token given by Frends.</param>
        /// <returns>Object { object Body, Dictionary&lt;string, string&gt; Headers, int StatusCode }</returns>
        public static async Task<Result> Request([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
        {
            var client = GetHttpClientForOptions(options);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            var headers = GetHeaderDictionary(input.Headers);

            using var content = GetContent(input, headers);
            using var responseMessage = await GetHttpRequestResponseAsync(
                    client,
                    input.Method.ToString(),
                    input.Url,
                    content,
                    headers,
                    cancellationToken)
                .ConfigureAwait(false);

            dynamic response;

#if NET6_0
            var rbody = TryParseRequestStringResultAsJToken(await responseMessage.Content
                .ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false));
#else
            var rbody = TryParseRequestStringResultAsJToken(await responseMessage.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false));
#endif

            var rstatusCode = (int)responseMessage.StatusCode;
            var rheaders = GetResponseHeaderDictionary(responseMessage.Headers, responseMessage.Content.Headers);
            response = new Result(rbody, rheaders, rstatusCode);

            if (!responseMessage.IsSuccessStatusCode && options.ThrowExceptionOnErrorResponse)
            {
                throw new WebException(
                    $"Request to '{input.Url}' failed with status code {(int)responseMessage.StatusCode}. Response body: {response.Body}");
            }

            return response;
        }

        private static IDictionary<string, string> GetHeaderDictionary(Header[] headers)
        {
            return headers.ToDictionary(key => key.Name, value => value.Value, StringComparer.InvariantCultureIgnoreCase);
        }

        private static HttpContent GetContent(Input input, IDictionary<string, string> headers)
        {
            // Check if Content-Type exists and is set and valid
            var contentTypeIsSetAndValid = false;
            MediaTypeWithQualityHeaderValue validContentType = null;
            if (headers.TryGetValue("content-type", out string contentTypeValue))
            {
                contentTypeIsSetAndValid = MediaTypeWithQualityHeaderValue.TryParse(contentTypeValue, out validContentType);
            }

            return contentTypeIsSetAndValid
                ? new StringContent(input.Message ?? string.Empty, Encoding.GetEncoding(validContentType.CharSet ?? Encoding.UTF8.WebName))
                : new StringContent(input.Message ?? string.Empty);
        }

        private static async Task<HttpResponseMessage> GetHttpRequestResponseAsync(
                HttpClient httpClient,
                string method,
                string url,
                HttpContent content,
                IDictionary<string, string> headers,
                CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Only POST, PUT, PATCH and DELETE can have content, otherwise the HttpClient will fail
            var isContentAllowed = Enum.TryParse(method, ignoreCase: true, result: out SendMethod _);

            using var request = new HttpRequestMessage(new HttpMethod(method), new Uri(url))
            {
                Content = isContentAllowed ? content : null,
            };

            foreach (var header in headers)
            {
                var requestHeaderAddedSuccessfully = request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                if (!requestHeaderAddedSuccessfully && request.Content != null)
                {
                    // Could not add to request headers try to add to content headers
                    // this check is probably not needed anymore as the new HttpClient does not seem fail on malformed headers
                    var contentHeaderAddedSuccessfully = content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    if (!contentHeaderAddedSuccessfully)
                    {
                        Trace.TraceWarning($"Could not add header {header.Key}:{header.Value}");
                    }
                }
            }

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
    }
}
