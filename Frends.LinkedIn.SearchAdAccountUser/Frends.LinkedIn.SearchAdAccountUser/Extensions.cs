using System;
using System.Net.Http;
using Frends.LinkedIn.SearchAdAccountUser.Definitions;

namespace Frends.LinkedIn.SearchAdAccountUser;

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
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("LinkedIn-Version", connection.LinkedInAPIVersion);
        httpClient.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(connection.ConnectionTimeoutSeconds));
    }
}
