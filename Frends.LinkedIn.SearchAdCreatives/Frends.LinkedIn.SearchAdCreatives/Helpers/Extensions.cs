using Frends.LinkedIn.SearchAdCreatives.Definitions;

namespace Frends.LinkedIn.SearchAdCreatives.Helpers;

internal static class Extensions
{
    internal static void SetHandlerSettingsBasedOnConnection(this HttpClientHandler handler, Connection connection)
    {
        handler.AllowAutoRedirect = connection.FollowRedirects;
        handler.UseCookies = connection.AutomaticCookieHandling;

        if (connection.AllowInvalidCertificate)
            handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
    }

    internal static void SetDefaultRequestHeadersBasedOnConnection(
        this HttpClient httpClient,
        Connection connection,
        bool getSingleCreative)
    {
        httpClient.DefaultRequestHeaders.ExpectContinue = false;
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("LinkedIn-Version", connection.LinkedInAPIVersion);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Restli-Protocol-Version", "2.0.0");
        if (!getSingleCreative)
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-RestLi-Method", "FINDER");

        httpClient.Timeout = TimeSpan.FromSeconds(connection.ConnectionTimeoutSeconds);
    }
}
