using System.Net.Http;

namespace Frends.LinkedIn.SearchAdCreatives.Definitions;

/// <summary>
/// Creates HTTP clients for LinkedIn requests.
/// </summary>
public interface IHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client configured with the supplied options.
    /// </summary>
    /// <param name="options">Options used to configure the client.</param>
    /// <returns>A configured HTTP client.</returns>
    HttpClient CreateClient(Options options);
}
