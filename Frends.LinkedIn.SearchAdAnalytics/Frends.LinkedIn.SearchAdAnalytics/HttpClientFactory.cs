// Copyright (c) Frends. All rights reserved.

using System.Net.Http;
using Frends.LinkedIn.SearchAdAnalytics.Definitions;

namespace Frends.LinkedIn.SearchAdAnalytics;

/// <summary>
/// Creates HTTP clients configured for LinkedIn requests.
/// </summary>
internal class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates a client configured from the task options.
    /// </summary>
    /// <param name="options">Task options.</param>
    /// <returns>A configured HTTP client.</returns>
    public HttpClient CreateClient(Options options)
    {
        var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnOptions(options);
        return new HttpClient(handler);
    }
}
