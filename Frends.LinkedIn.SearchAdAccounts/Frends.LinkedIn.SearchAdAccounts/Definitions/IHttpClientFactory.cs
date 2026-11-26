namespace Frends.LinkedIn.SearchAdAccounts.Definitions;

/// <summary>
/// Creates HTTP clients for LinkedIn requests.
/// </summary>
public interface IHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client using the supplied connection settings.
    /// </summary>
    /// <param name="connection">Connection settings.</param>
    /// <returns>A configured HTTP client.</returns>
    HttpClient CreateClient(Connection connection);
}
