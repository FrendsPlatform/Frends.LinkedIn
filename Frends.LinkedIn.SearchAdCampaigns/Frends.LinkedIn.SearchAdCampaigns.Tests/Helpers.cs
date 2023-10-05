namespace Frends.LinkedIn.SearchAdCampaigns.Tests;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

internal static class Helpers
{
    internal static async Task<string> GetAccessToken(string clientId, string clientSecret)
    {
        // Construct the URL for token exchange
        string tokenUrl = "https://www.linkedin.com/oauth/v2/accessToken";
        var tokenRequestData = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", clientId },
            { "client_secret", clientSecret },
        };

        using HttpClient httpClient = new HttpClient();

        // Send a POST request to the token endpoint
        var response = await httpClient.PostAsync(tokenUrl, new FormUrlEncodedContent(tokenRequestData));

        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LinkedInTokenResponse>(responseContent);

            string accessToken = tokenResponse.AccessToken;
            return accessToken;
        }
        else
        {
            throw new ArgumentException($"Token request failed. Status code: {responseContent}");
        }
    }

    public class LinkedInTokenResponse
    {
        public string AccessToken { get; set; }

        public string ExpiresIn { get; set; }
    }
}