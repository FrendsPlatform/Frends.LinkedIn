namespace Frends.LinkedIn.SearchAdAccounts;

/// <summary>
/// Creates HTTP clients configured for LinkedIn requests.
/// </summary>
internal class HttpClientFactory : LinkedInHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client using the supplied connection settings.
    /// </summary>
    /// <param name="connection">Connection settings.</param>
    /// <returns>A configured HTTP client.</returns>
    public HttpClient CreateClient(Connection connection)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnConnection(connection);
        return new HttpClient(handler);
    }
}
