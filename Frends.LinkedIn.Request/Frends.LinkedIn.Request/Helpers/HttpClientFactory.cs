using System.Net.Http;
using Frends.LinkedIn.Request.Definitions;

namespace Frends.LinkedIn.Request;

/// <summary>
/// Creates HTTP clients configured for LinkedIn requests.
/// </summary>
internal class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates an HTTP client using the supplied options.
    /// </summary>
    /// <param name="options">The task options.</param>
    /// <returns>The configured HTTP client.</returns>
    public HttpClient CreateClient(Options options)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnOptions(options);
        return new HttpClient(handler);
    }
}
