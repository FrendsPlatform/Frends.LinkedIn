namespace Frends.LinkedIn.SearchAdAccounts.Helpers;

/// <summary>
/// Extensions for configuring LinkedIn HTTP requests.
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Configures the HTTP handler using task connection.
    /// </summary>
    /// <param name="handler">The HTTP handler to configure.</param>
    /// <param name="connection">The task connection.</param>
    internal static void SetHandlerSettingsBasedOnConnection(this HttpClientHandler handler, Connection connection)
    {
        handler.AllowAutoRedirect = connection.FollowRedirects;
        handler.UseCookies = connection.AutomaticCookieHandling;

        if (connection.AllowInvalidCertificate)
        {
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        }
    }

    /// <summary>
    /// Configures default headers and timeout using task connection.
    /// </summary>
    /// <param name="httpClient">The HTTP client to configure.</param>
    /// <param name="connection">The task connection.</param>
    internal static void SetDefaultRequestHeadersBasedOnConnection(this HttpClient httpClient, Connection connection)
    {
        httpClient.DefaultRequestHeaders.ExpectContinue = false;
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("LinkedIn-Version", connection.LinkedInAPIVersion);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Restli-Protocol-Version", "2.0.0");
        httpClient.Timeout = TimeSpan.FromSeconds(connection.ConnectionTimeoutSeconds);
    }
}
