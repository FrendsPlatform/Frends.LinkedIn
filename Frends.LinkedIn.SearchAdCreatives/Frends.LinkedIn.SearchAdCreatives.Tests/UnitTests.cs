namespace Frends.LinkedIn.SearchAdCreatives.Tests;

using System;
using System.Threading.Tasks;
using Frends.LinkedIn.SearchAdCreatives.Definitions;
using NUnit.Framework;

[TestFixture]
internal class UnitTests
{
    private static Filter _input;
    private static Options _options;

    [SetUp]
    public void Setup()
    {
        var accessToken = Environment.GetEnvironmentVariable("Frends_LinkedIn_AccessToken");

        _input = new Filter
        {
            AdAccountId = "512500029",
            CampaignUrn = string.Empty,
        };

        _options = new Options
        {
            LinkedInAPIVersion = "202309",
            Token = accessToken,
            ThrowExceptionOnErrorResponse = true,
            ConnectionTimeoutSeconds = 30,
            AllowInvalidCertificate = true,
            AllowInvalidResponseContentTypeCharSet = true,
            AutomaticCookieHandling = true,
            FollowRedirects = true,
        };
    }

    [Test]
    public async Task SearchCampaigns_TestGetAllCampaigns()
    {
        var result = await LinkedIn.SearchAdCreatives(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public async Task SearchCampaigns_TestGetCampaignsWithId()
    {
        _input.CampaignUrn = "urn:li:organization:99966515";
        var result = await LinkedIn.SearchAdCreatives(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
    }
}
