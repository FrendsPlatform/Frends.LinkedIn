namespace Frends.LinkedIn.SearchAdAnalytics;

using System.Net.Http;
using Frends.LinkedIn.SearchAdAnalytics.Definitions;

internal class HttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(Options options)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnOptions(options);
        return new HttpClient(handler);
    }
}
