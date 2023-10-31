﻿namespace Frends.LinkedIn.SearchAdAccountUser
{
    using System;
    using System.Net.Http;
    using Frends.LinkedIn.SearchAdAccountUser.Definitions;

    internal static class Extensions
    {
        internal static void SetHandlerSettingsBasedOnOptions(this HttpClientHandler handler, Options options)
        {
            handler.AllowAutoRedirect = options.FollowRedirects;
            handler.UseCookies = options.AutomaticCookieHandling;

            if (options.AllowInvalidCertificate)
                handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
        }

        internal static void SetDefaultRequestHeadersBasedOnOptions(this HttpClient httpClient, Options options)
        {
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("LinkedIn-Version", options.LinkedInAPIVersion);
            httpClient.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(options.ConnectionTimeoutSeconds));
        }
    }
}