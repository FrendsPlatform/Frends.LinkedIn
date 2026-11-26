using Frends.LinkedIn.SearchAdCampaigns.Definitions;

namespace Frends.LinkedIn.SearchAdCampaigns;

internal static class Extensions
{
    internal static void SetHandlerSettingsBasedOnConnection(this HttpClientHandler handler, Connection connection)
    {
        handler.AllowAutoRedirect = connection.FollowRedirects;
        handler.UseCookies = connection.AutomaticCookieHandling;

        if (connection.AllowInvalidCertificate)
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
    }

    internal static void SetDefaultRequestHeadersBasedOnConnection(this HttpClient httpClient, Connection connection)
    {
        httpClient.DefaultRequestHeaders.ExpectContinue = false;
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("LinkedIn-Version", connection.LinkedInAPIVersion);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Restli-Protocol-Version", "2.0.0");
        httpClient.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(connection.ConnectionTimeoutSeconds));
    }
}
