using Frends.LinkedIn.SearchAdCreatives.Definitions;

namespace Frends.LinkedIn.SearchAdCreatives.Helpers;

/// <summary>
/// Creates HTTP clients configured for LinkedIn requests.
/// </summary>
internal class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client with the configured handler.
    /// </summary>
    /// <param name="connection">Connection settings used to configure the handler.</param>
    /// <returns>A configured HTTP client.</returns>
    public HttpClient CreateClient(Connection connection)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnConnection(connection);
        return new HttpClient(handler);
    }
}
