namespace Frends.LinkedIn.SearchAdAccountUser
{
    using System.Net.Http;
    using Frends.LinkedIn.SearchAdAccountUser.Definitions;

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
