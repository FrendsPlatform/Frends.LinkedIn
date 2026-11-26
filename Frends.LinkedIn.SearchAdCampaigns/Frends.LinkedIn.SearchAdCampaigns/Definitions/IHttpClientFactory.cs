namespace Frends.LinkedIn.SearchAdCampaigns.Definitions;

/// <summary>
/// Http Client Factory Interface.
/// </summary>
public interface IHttpClientFactory
{
    /// <summary>
    /// Create client.
    /// </summary>
    /// <param name="connection">Connection parameters.</param>
    /// <returns>HttpClient object.</returns>
    HttpClient CreateClient(Connection connection);
}
