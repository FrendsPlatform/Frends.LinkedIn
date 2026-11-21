using Frends.LinkedIn.SearchAdCampaigns.Definitions;

namespace Frends.LinkedIn.SearchAdCampaigns;

internal static class Extensions
{
    internal static void SetHandlerSettingsBasedOnOptions(this HttpClientHandler handler, Options options)
    {
        handler.AllowAutoRedirect = options.FollowRedirects;
        handler.UseCookies = options.AutomaticCookieHandling;

        if (options.AllowInvalidCertificate)
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
    }

    internal static void SetDefaultRequestHeadersBasedOnOptions(this HttpClient httpClient, Options options)
    {
        httpClient.DefaultRequestHeaders.ExpectContinue = false;
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("LinkedIn-Version", options.LinkedInAPIVersion);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Restli-Protocol-Version", "2.0.0");
        httpClient.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(options.ConnectionTimeoutSeconds));
    }
}
