namespace Frends.LinkedIn.SearchAdAccounts.Tests;

using System.Threading.Tasks;
using Frends.LinkedIn.SearchAdAccounts.Definitions;
using NUnit.Framework;

[TestFixture]
internal class UnitTests
{
    private static Filter _input;
    private static Options _options;

    [SetUp]
    public void Setup()
    {
        var accessToken = "";

        _input = new Filter
        {
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
        var result = await LinkedIn.SearchAdAccounts(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public async Task SearchCampaigns_TestGetCampaignsWithId()
    {
        _input.Id = "512500029";
        var result = await LinkedIn.SearchAdAccounts(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public async Task SearchCampaigns_TestGetCampaignsWithName()
    {
        _input.Name = "FrendsTasks Ad Account";
        var result = await LinkedIn.SearchAdAccounts(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public async Task SearchCampaigns_TestGetActiveTextAdCampaigns()
    {
        _input.TypeFilters = new TypeFilter[]
            {
                new TypeFilter { AccountType = AccountType.ENTERPRISE },
            };
        var result = await LinkedIn.SearchAdAccounts(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsEmpty(result.Body.elements);
    }

    [Test]
    public async Task SearchCampaigns_TestGetAllDraftCampaigns()
    {
        _input.StatusFilters = new StatusFilter[]
            {
                new StatusFilter { AccountStatus = AccountStatus.DRAFT },
            };
        var result = await LinkedIn.SearchAdAccounts(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public async Task SearchCampaigns_TestGetAllActiveCampaigns()
    {
        _input.StatusFilters = new StatusFilter[]
            {
                new StatusFilter { AccountStatus = AccountStatus.ACTIVE },
            };
        var result = await LinkedIn.SearchAdAccounts(_input, _options, default);
        Assert.AreEqual(200, result.StatusCode);
    }
}
