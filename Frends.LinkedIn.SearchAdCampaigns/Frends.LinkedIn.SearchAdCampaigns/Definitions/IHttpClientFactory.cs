namespace Frends.LinkedIn.SearchAdCampaigns.Definitions;

/// <summary>
/// Http Client Factory Interface.
/// </summary>
public interface IHttpClientFactory
{
    /// <summary>
    /// Create client.
    /// </summary>
    /// <param name="options">Option parameters.</param>
    /// <returns>HttpClient object.</returns>
    HttpClient CreateClient(Options options);
}
