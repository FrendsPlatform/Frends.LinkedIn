namespace Frends.LinkedIn.Request
{
    using System.Net.Http;
    using Frends.LinkedIn.Request.Definitions;

    internal class HttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// Creates an HTTP client using the provided options.
        /// </summary>
        /// <param name="options">Options that configure the HTTP client.</param>
        /// <returns>A configured HTTP client.</returns>
        public HttpClient CreateClient(Options options)
        {
            var handler = new HttpClientHandler();
            handler.SetHandlerSettingsBasedOnOptions(options);
            return new HttpClient(handler);
        }
    }
}
