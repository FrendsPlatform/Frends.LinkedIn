using Frends.LinkedIn.SearchAdCampaigns.Definitions;

namespace Frends.LinkedIn.SearchAdCampaigns;

/// <summary>
/// Default factory creating HttpClients based on task options.
/// </summary>
internal class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates an HttpClient configured according to the connection parameters.
    /// </summary>
    /// <param name="connection">Connection parameters.</param>
    /// <returns>HttpClient object.</returns>
    public HttpClient CreateClient(Connection connection)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnConnection(connection);
        return new HttpClient(handler);
    }
}
