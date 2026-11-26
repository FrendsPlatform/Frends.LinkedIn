using System.Net.Http;
using Frends.LinkedIn.SearchAdAnalytics.Definitions;

namespace Frends.LinkedIn.SearchAdAnalytics;

/// <summary>
/// Creates HTTP clients configured for LinkedIn requests.
/// </summary>
internal class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates a client configured from the connection parameters.
    /// </summary>
    /// <param name="connection">Connection parameters.</param>
    /// <returns>A configured HTTP client.</returns>
    public HttpClient CreateClient(Connection connection)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnConnection(connection);
        return new HttpClient(handler);
    }
}
