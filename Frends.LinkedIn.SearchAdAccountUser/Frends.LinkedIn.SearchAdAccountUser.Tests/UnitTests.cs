namespace Frends.LinkedIn.SearchAdAccountUser.Tests;

using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Frends.LinkedIn.SearchAdAccountUser.Definitions;
using NUnit.Framework;

[TestFixture]
internal class UnitTests
{
    private static Filter _filter;
    private static Options _options;

    [SetUp]
    public void Setup()
    {
        var accessToken = Environment.GetEnvironmentVariable("Frends_LinkedIn_AccessToken");

        _filter = new Filter
        {
            SearchMethod = SearchAttribute.FindAdAccountsByAuthenticatedUser,
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
    public async Task LinkedIn_TestFindAdAccountsByAuthenticatedUser()
    {
        var result = await LinkedIn.SearchAdAccountUser(_filter, _options, default);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public async Task LinkedIn_TestFindAdAccountUsersByAccounts()
    {
        var accounts = await LinkedIn.SearchAdAccountUser(_filter, _options, default);
        var urn = accounts.Body.elements[0].account.ToString();

        _filter.SearchMethod = SearchAttribute.FindAdAccountUsersByAccounts;
        _filter.AccountUrns = new AccountUrn[] { new AccountUrn { Urn = urn } };
        var result = await LinkedIn.SearchAdAccountUser(_filter, _options, default);
        Assert.AreEqual(200, result.StatusCode);
    }

    [Test]
    public async Task LinkedIn_TestGetAdAccountUser()
    {
        var accounts = await LinkedIn.SearchAdAccountUser(_filter, _options, default);
        var accountUrn = accounts.Body.elements[0].account.ToString();
        var userUrn = accounts.Body.elements[0].user.ToString();

        _filter.SearchMethod = SearchAttribute.GetAdAccountUser;
        _filter.AccountUrn = accountUrn;
        _filter.UserUrn = userUrn;
        var result = await LinkedIn.SearchAdAccountUser(_filter, _options, default);
        Assert.AreEqual(200, result.StatusCode);
    }
}
