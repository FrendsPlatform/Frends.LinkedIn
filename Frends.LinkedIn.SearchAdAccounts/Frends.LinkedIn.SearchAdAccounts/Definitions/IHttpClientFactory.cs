namespace Frends.LinkedIn.SearchAdAccounts.Definitions;

/// <summary>
/// Creates HTTP clients for LinkedIn requests.
/// </summary>
public interface IHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client using the supplied options.
    /// </summary>
    /// <param name="options">Request options.</param>
    /// <returns>A configured HTTP client.</returns>
    HttpClient CreateClient(Options options);
}
