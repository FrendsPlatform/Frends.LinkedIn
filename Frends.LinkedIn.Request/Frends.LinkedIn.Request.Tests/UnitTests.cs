namespace Frends.LinkedIn.Request.Tests;

using System;
using System.Threading.Tasks;
using Frends.LinkedIn.Request.Definitions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

[TestFixture]
internal class UnitTests
{
    private static Options options;

    /// <summary>
    /// Initializes options used by the tests.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        var accessToken = Environment.GetEnvironmentVariable("Frends_LinkedIn_AccessToken");

        options = new Options
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

    /// <summary>
    /// Sends requests to LinkedIn and verifies successful responses.
    /// </summary>
    /// <returns>A task representing the asynchronous test.</returns>
    [Test]
    public async Task Test()
    {
        var urls = new string[]
        {
            "https://api.linkedin.com/rest/adAccounts?q=search&search=(type:(values:List(ENTERPRISE)))",
            "https://api.linkedin.com/rest/adAccounts?q=search&search=(status:(values:List(ACTIVE)))",
            "https://api.linkedin.com/rest/adAccounts?q=search&search=()",
            "https://api.linkedin.com/rest/adAccounts?q=search&search=(status:(values:List(DRAFT)))",
            "https://api.linkedin.com/rest/adAccounts?q=search&search=(id:(values:List(512500029)))",
            "https://api.linkedin.com/rest/adAccounts?q=search&search=(name:(values:List(FrendsTasks Ad Account)))",
            "https://api.linkedin.com/rest/adAccounts/512500029/creatives?campaigns=List(urn%3ali%3asponsoredCampaign%3a99966515)&q=criteria",
            "https://api.linkedin.com/rest/adAccounts/512500029/creatives?creatives=List(urn%3ali%3asponsoredCreative%3a119962155)&q=criteria",
        };

        Input input;

        foreach (var url in urls)
        {
            input = new Input
            {
                Method = Method.GET,
                Url = url,
                Headers = Array.Empty<Header>(),
            };

            var result = await LinkedIn.Request(input, options, default);
            ClassicAssert.AreEqual(200, result.StatusCode);
        }
    }

    [Test]
    public void RequestFailureReturnsErrorWhenConfigured()
    {
        var exception = new InvalidOperationException("Request failed.");
        var options = new Options
        {
            ThrowErrorOnFailure = false,
            ErrorMessageOnFailure = "Custom message",
        };

        var result = exception.Handle(options);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Is.EqualTo("Custom message: Request failed."));
        Assert.That(result.Error.AdditionalInfo, Is.SameAs(exception));
    }

    [Test]
    public void RequestFailureThrowsWhenConfigured()
    {
        var exception = new InvalidOperationException("Request failed.");

        var actual = Assert.Throws<InvalidOperationException>(() => exception.Handle(new Options()));

        Assert.That(actual, Is.SameAs(exception));
    }
}
