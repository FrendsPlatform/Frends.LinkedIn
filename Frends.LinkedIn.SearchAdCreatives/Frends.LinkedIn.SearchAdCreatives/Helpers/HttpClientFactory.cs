using Frends.LinkedIn.SearchAdCreatives.Definitions;

namespace Frends.LinkedIn.SearchAdCreatives.Helpers;

/// <summary>
/// Creates HTTP clients configured for LinkedIn requests.
/// </summary>
internal class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client with the configured handler.
    /// </summary>
    /// <param name="options">Options used to configure the handler.</param>
    /// <returns>A configured HTTP client.</returns>
    public HttpClient CreateClient(Options options)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnOptions(options);
        return new HttpClient(handler);
    }
}
