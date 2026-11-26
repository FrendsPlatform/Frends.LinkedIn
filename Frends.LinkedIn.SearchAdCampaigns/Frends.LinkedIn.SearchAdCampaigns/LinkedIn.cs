using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Frends.LinkedIn.SearchAdCampaigns.Definitions;
using Frends.LinkedIn.SearchAdCampaigns.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frends.LinkedIn.SearchAdCampaigns;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class LinkedIn
{
    internal static IHttpClientFactory ClientFactory { get; set; } = new HttpClientFactory();

    /// <summary>
    /// Frends Task for finding LinkedIn Campaigns using LinkedIn Rest API.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.LinkedIn.SearchAdCampaigns).
    /// </summary>
    /// <param name="input">Essential parameters.</param>
    /// <param name="connection">Connection parameters.</param>
    /// <param name="options">Additional parameters.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>object { bool Success, object Body, Dictionary(string, string) Headers, int StatusCode, object Error { string Message, Exception AdditionalInfo } }.</returns>
    public static async Task<Result> SearchAdCampaigns(
        [PropertyTab] Input input,
        [PropertyTab] Connection connection,
        [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            var uri = FormRequestURI(input);

            using var client = ClientFactory.CreateClient(connection);
            client.SetDefaultRequestHeadersBasedOnConnection(connection);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", connection.Token);

            using var responseMessage = await GetHttpRequestResponseAsync(client, uri, cancellationToken)
                .ConfigureAwait(false);

            var body = TryParseRequestStringResultAsJToken(await responseMessage.Content
                .ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false));

            var statusCode = (int)responseMessage.StatusCode;

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new WebException(
                    $"Request to '{uri}' failed with status code {statusCode}. Response body: {body}");
            }

            return new Result
            {
                Success = true,
                Body = body,
                Headers = GetResponseHeaderDictionary(responseMessage.Headers, responseMessage.Content.Headers),
                StatusCode = statusCode,
                Error = null,
            };
        }
        catch (Exception ex)
        {
            return ex.Handle(options);
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

            // Cancellation is from inside of the request, mostly likely a timeout
            throw new ArgumentException("HttpRequest was canceled, most likely due to a timeout.", canceledException);
        }
    }

    private static JToken TryParseRequestStringResultAsJToken(string response)
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

    private static string FormRequestURI(Input input)
    {
        var sb = new StringBuilder($"https://api.linkedin.com/rest/adAccounts/{input.AdAccountId}/adCampaigns");

        sb.Append("?q=search&search=(");

        var dict = new Dictionary<string, List<string>>();

        if (!string.IsNullOrEmpty(input.Id))
            dict["id"] = new List<string> { input.Id };

        if (!string.IsNullOrEmpty(input.Name))
            dict["name"] = new List<string> { input.Name };

        if (!string.IsNullOrEmpty(input.Reference))
            dict["reference"] = new List<string> { input.Reference };

        var typeFilters = input.TypeFilters != null ? input.TypeFilters.Select(x => x.CampaignType.ToString()).ToList() : new List<string>();
        if (typeFilters.Count > 0)
            dict["type"] = typeFilters;

        var statusFilters = input.StatusFilters != null ? input.StatusFilters.Select(x => x.CampaignStatus.ToString()).ToList() : new List<string>();
        if (statusFilters.Count > 0)
            dict["status"] = statusFilters;

        foreach (var obj in dict)
            sb.Append($"{obj.Key}:(values:List({string.Join(",", obj.Value)})),");

        if (sb.ToString().EndsWith(','))
            sb.Remove(sb.Length - 1, 1);

        sb.Append(')');

        return sb.ToString();
    }
}
