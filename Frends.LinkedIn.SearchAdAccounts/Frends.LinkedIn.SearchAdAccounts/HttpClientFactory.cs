namespace Frends.LinkedIn.SearchAdAccounts
{
    using System.Net.Http;
    using Frends.LinkedIn.SearchAdAccounts.Definitions;

    internal class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(Options options)
        {
            var handler = new HttpClientHandler();
            handler.SetHandlerSettingsBasedOnOptions(options);
            return new HttpClient(handler);
        }
    }
}
