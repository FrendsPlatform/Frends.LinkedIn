namespace Frends.LinkedIn.SearchAdAccounts;

/// <summary>
/// Creates HTTP clients configured for LinkedIn requests.
/// </summary>
internal class HttpClientFactory : LinkedInHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client using the supplied options.
    /// </summary>
    /// <param name="options">Request options.</param>
    /// <returns>A configured HTTP client.</returns>
    public HttpClient CreateClient(Options options)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnOptions(options);
        return new HttpClient(handler);
    }
}
