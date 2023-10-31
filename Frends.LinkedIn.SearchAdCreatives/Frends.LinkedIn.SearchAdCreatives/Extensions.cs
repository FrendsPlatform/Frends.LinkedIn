namespace Frends.LinkedIn.SearchAdCreatives
{
    using System;
    using System.Net.Http;
    using Frends.LinkedIn.SearchAdCreatives.Definitions;

    internal static class Extensions
    {
        internal static void SetHandlerSettingsBasedOnOptions(this HttpClientHandler handler, Options options)
        {
            handler.AllowAutoRedirect = options.FollowRedirects;
            handler.UseCookies = options.AutomaticCookieHandling;

            if (options.AllowInvalidCertificate)
                handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
        }

        internal static void SetDefaultRequestHeadersBasedOnOptions(this HttpClient httpClient, Options options, bool getSingleCreative)
        {
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("LinkedIn-Version", options.LinkedInAPIVersion);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Restli-Protocol-Version", "2.0.0");
            if (!getSingleCreative)
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-RestLi-Method", "FINDER");
            }
            httpClient.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(options.ConnectionTimeoutSeconds));
        }
    }
}
