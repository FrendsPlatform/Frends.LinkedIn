using System.Net.Http;
using Frends.LinkedIn.Request.Definitions;

namespace Frends.LinkedIn.Request;

/// <summary>
/// Creates HTTP clients configured for LinkedIn requests.
/// </summary>
internal class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client using the supplied connection.
    /// </summary>
    /// <param name="connection">The connection parameters.</param>
    /// <returns>The configured HTTP client.</returns>
    public HttpClient CreateClient(Connection connection)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnConnection(connection);
        return new HttpClient(handler);
    }
}
