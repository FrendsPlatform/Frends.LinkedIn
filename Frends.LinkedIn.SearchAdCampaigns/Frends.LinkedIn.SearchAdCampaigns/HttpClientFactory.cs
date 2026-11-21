using Frends.LinkedIn.SearchAdCampaigns.Definitions;

namespace Frends.LinkedIn.SearchAdCampaigns;

/// <summary>
/// Default factory creating HttpClients based on task options.
/// </summary>
internal class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates an HttpClient configured according to the options.
    /// </summary>
    /// <param name="options">Option parameters.</param>
    /// <returns>HttpClient object.</returns>
    public HttpClient CreateClient(Options options)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnOptions(options);
        return new HttpClient(handler);
    }
}
