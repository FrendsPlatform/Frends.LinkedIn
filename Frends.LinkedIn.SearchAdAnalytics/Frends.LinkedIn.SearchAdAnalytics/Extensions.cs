// Copyright (c) Frends. All rights reserved.

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
    /// Applies transport settings from the task options to an HTTP handler.
    /// </summary>
    /// <param name="handler">The HTTP handler to configure.</param>
    /// <param name="options">The task options.</param>
    internal static void SetHandlerSettingsBasedOnOptions(this HttpClientHandler handler, Options options)
    {
        handler.AllowAutoRedirect = options.FollowRedirects;
        handler.UseCookies = options.AutomaticCookieHandling;

        if (options.AllowInvalidCertificate)
        {
            handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
        }
    }

    /// <summary>
    /// Applies default request headers and timeout from the task options.
    /// </summary>
    /// <param name="httpClient">The HTTP client to configure.</param>
    /// <param name="options">The task options.</param>
    internal static void SetDefaultRequestHeadersBasedOnOptions(this HttpClient httpClient, Options options)
    {
        httpClient.DefaultRequestHeaders.ExpectContinue = false;
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("LinkedIn-Version", options.LinkedInAPIVersion);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Restli-Protocol-Version", "2.0.0");
        httpClient.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(options.ConnectionTimeoutSeconds));
    }
}
