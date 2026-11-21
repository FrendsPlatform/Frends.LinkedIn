using System.Net.Http;

namespace Frends.LinkedIn.SearchAdAccountUser.Definitions;

/// <summary>
/// HTTP client factory interface.
/// </summary>
public interface IHttpClientFactory
{
    /// <summary>
    /// Create an HTTP client.
    /// </summary>
    /// <param name="options">Task options.</param>
    /// <returns>An HTTP client.</returns>
    HttpClient CreateClient(Options options);
}
