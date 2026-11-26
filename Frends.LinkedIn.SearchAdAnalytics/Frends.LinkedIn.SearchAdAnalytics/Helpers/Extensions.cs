using System;
using System.Net.Http;
using Frends.LinkedIn.SearchAdAnalytics.Definitions;

namespace Frends.LinkedIn.SearchAdAnalytics;

/// <summary>
/// Extensions for configuring LinkedIn HTTP requests.
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Applies transport settings from the connection parameters to an HTTP handler.
    /// </summary>
    /// <param name="handler">The HTTP handler to configure.</param>
    /// <param name="connection">The connection parameters.</param>
    internal static void SetHandlerSettingsBasedOnConnection(this HttpClientHandler handler, Connection connection)
    {
        handler.AllowAutoRedirect = connection.FollowRedirects;
        handler.UseCookies = connection.AutomaticCookieHandling;

        if (connection.AllowInvalidCertificate)
        {
            handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
        }
    }

    /// <summary>
    /// Applies default request headers and timeout from the connection parameters.
    /// </summary>
    /// <param name="httpClient">The HTTP client to configure.</param>
    /// <param name="connection">The connection parameters.</param>
    internal static void SetDefaultRequestHeadersBasedOnConnection(this HttpClient httpClient, Connection connection)
    {
        httpClient.DefaultRequestHeaders.ExpectContinue = false;
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("LinkedIn-Version", connection.LinkedInAPIVersion);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Restli-Protocol-Version", "2.0.0");
        httpClient.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(connection.ConnectionTimeoutSeconds));
    }
}
