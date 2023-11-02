namespace Frends.LinkedIn.SearchAdCampaigns.Tests;

using System;
using System.Threading.Tasks;
using Frends.LinkedIn.SearchAdCampaigns.Definitions;
using NUnit.Framework;

/// <summary>
/// Test class.
/// </summary>
[TestFixture]
internal class TestClass
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
            Id = string.Empty,
            Name = string.Empty,
            Reference = string.Empty,
            TypeFilters = null,
            StatusFilters = null,
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
        var result = await LinkedIn.SearchAdCampaigns(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
        Console.WriteLine(result.Body);
    }

    [Test]
    public async Task SearchCampaigns_TestGetCampaignsWithId()
    {
        _input.Id = "277937223";
        var result = await LinkedIn.SearchAdCampaigns(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
        Console.WriteLine(result.Body);
    }

    [Test]
    public async Task SearchCampaigns_TestGetCampaignsWithName()
    {
        _input.Name = "Test campaign";
        var result = await LinkedIn.SearchAdCampaigns(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
        Console.WriteLine(result.Body);
    }

    [Test]
    public async Task SearchCampaigns_TestGetActiveTextAdCampaigns()
    {
        _input.TypeFilters = new TypeFilter[]
            {
                new TypeFilter { CampaignType = CampaignType.TEXT_AD },
            };
        var result = await LinkedIn.SearchAdCampaigns(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsEmpty(result.Body.elements);
    }

    [Test]
    public async Task SearchCampaigns_TestGetAllDraftCampaigns()
    {
        _input.StatusFilters = new StatusFilter[]
            {
                new StatusFilter { CampaignStatus = CampaignStatus.DRAFT },
            };
        var result = await LinkedIn.SearchAdCampaigns(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public async Task SearchCampaigns_TestGetAllActiveCampaigns()
    {
        _input.StatusFilters = new StatusFilter[]
            {
                new StatusFilter { CampaignStatus = CampaignStatus.ACTIVE },
            };
        var result = await LinkedIn.SearchAdCampaigns(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsEmpty(result.Body.elements);
    }
}
