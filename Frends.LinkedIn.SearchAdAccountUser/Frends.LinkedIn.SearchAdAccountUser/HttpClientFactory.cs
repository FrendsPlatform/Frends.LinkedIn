using System.Net.Http;
using Frends.LinkedIn.SearchAdAccountUser.Definitions;

namespace Frends.LinkedIn.SearchAdAccountUser;

/// <summary>
/// Creates HTTP clients configured for LinkedIn requests.
/// </summary>
internal class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client configured with the supplied task options.
    /// </summary>
    /// <param name="options">Task options.</param>
    /// <returns>A configured HTTP client.</returns>
    public HttpClient CreateClient(Options options)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnOptions(options);
        return new HttpClient(handler);
    }
}
