// <copyright file="Extensions.cs" company="Frends">
// Copyright (c) Frends. All rights reserved.
// </copyright>

namespace Frends.LinkedIn.SearchAdAccounts;

/// <summary>
/// Extensions for configuring LinkedIn HTTP requests.
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Configures the HTTP handler using task options.
    /// </summary>
    /// <param name="handler">The HTTP handler to configure.</param>
    /// <param name="options">The task options.</param>
    internal static void SetHandlerSettingsBasedOnOptions(this HttpClientHandler handler, Options options)
    {
        handler.AllowAutoRedirect = options.FollowRedirects;
        handler.UseCookies = options.AutomaticCookieHandling;

        if (options.AllowInvalidCertificate)
        {
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        }
    }

    /// <summary>
    /// Configures default headers and timeout using task options.
    /// </summary>
    /// <param name="httpClient">The HTTP client to configure.</param>
    /// <param name="options">The task options.</param>
    internal static void SetDefaultRequestHeadersBasedOnOptions(this HttpClient httpClient, Options options)
    {
        httpClient.DefaultRequestHeaders.ExpectContinue = false;
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("LinkedIn-Version", options.LinkedInAPIVersion);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Restli-Protocol-Version", "2.0.0");
        httpClient.Timeout = TimeSpan.FromSeconds(options.ConnectionTimeoutSeconds);
    }
}
