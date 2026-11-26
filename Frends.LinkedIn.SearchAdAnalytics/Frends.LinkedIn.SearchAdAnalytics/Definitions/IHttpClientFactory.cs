using System.Net.Http;

namespace Frends.LinkedIn.SearchAdAnalytics.Definitions;

/// <summary>
/// HTTP client factory interface.
/// </summary>
public interface IHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client.
    /// </summary>
    /// <param name="connection">Connection parameters.</param>
    /// <returns>An HTTP client.</returns>
    HttpClient CreateClient(Connection connection);
}
