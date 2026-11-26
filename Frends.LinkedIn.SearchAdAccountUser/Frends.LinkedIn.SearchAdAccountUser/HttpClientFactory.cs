using System.Net.Http;
using Frends.LinkedIn.SearchAdAccountUser.Definitions;

namespace Frends.LinkedIn.SearchAdAccountUser;

/// <summary>
/// Creates HTTP clients configured for LinkedIn requests.
/// </summary>
internal class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client configured with the supplied connection parameters.
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
