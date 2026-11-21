using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Frends.LinkedIn.SearchAdAnalytics.Definitions;
using NUnit.Framework;

namespace Frends.LinkedIn.SearchAdAnalytics.Tests;

/// <summary>
/// Unit tests for SearchAdAnalytics.
/// </summary>
[TestFixture]
internal class UnitTests
{
    private static readonly RecordingHandler Handler = new();

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        LinkedIn.ClientFactory = new MockHttpClientFactory(Handler);
    }

    [SetUp]
    public void Setup()
    {
        Handler.ThrowTimeout = false;
        Handler.Response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"elements\":[]}"),
        };
        Handler.Response.Headers.TryAddWithoutValidation("X-Test", "header");
    }

    [Test]
    public async Task SearchAdAnalytics_AnalyticsRequestReturnsResponse()
    {
        var result = await LinkedIn.SearchAdAnalytics(CreateInput(), CreateOptions(), default);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Error, Is.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Headers["X-Test"], Is.EqualTo("header"));
        Assert.That(Handler.LastRequestUri.AbsolutePath, Is.EqualTo("/rest/adAnalytics"));
        Assert.That(Handler.LastRequestUri.Query, Does.Contain("q=analytics"));
        Assert.That(Handler.LastAuthorizationHeader, Does.EndWith("sample-access-token"));
    }

    [Test]
    public async Task SearchAdAnalytics_IncludesAnalyticsFilters()
    {
        var input = CreateInput();
        input.Pivot = Pivot.COMPANY;
        input.EndDate = "2023-12-11";
        input.TimeGranularity = TimeGranularity.DAILY;
        input.Campaigns = new[] { "urn:li:sponsoredCampaign:1234567" };
        input.Creatives = new[] { "urn:li:sponsoredCreative:1234567" };
        input.Metrics = new[] { "clicks", "comments" };

        await LinkedIn.SearchAdAnalytics(input, CreateOptions(), default);

        Assert.That(Handler.LastRequestUri.Query, Does.Contain("pivot=COMPANY"));
        Assert.That(Handler.LastRequestUri.Query, Does.Contain("dateRange=(start:"));
        Assert.That(Handler.LastRequestUri.Query, Does.Contain("dateRange=(end:"));
        Assert.That(Handler.LastRequestUri.Query, Does.Contain("campaigns=List"));
        Assert.That(Handler.LastRequestUri.Query, Does.Contain("creatives=List"));
        Assert.That(Handler.LastRequestUri.Query, Does.Contain("fields=clicks,comments"));
        Assert.That(Handler.LastRequestUri.Query, Does.Contain("timeGranularity=DAILY"));
    }

    [Test]
    public async Task SearchAdAnalytics_StatisticsRequestIncludesPivots()
    {
        var input = CreateInput();
        input.Finder = Finder.Statistics;
        input.Pivots = new[] { Pivot.COMPANY, Pivot.ACCOUNT };

        await LinkedIn.SearchAdAnalytics(input, CreateOptions(), default);

        Assert.That(Handler.LastRequestUri.Query, Does.Contain("q=statistics"));
        Assert.That(Handler.LastRequestUri.Query, Does.Contain("pivots=List(COMPANY,ACCOUNT)"));
    }

    [TestCase("invalid", "2023-12-11", "StartDate")]
    [TestCase("2023-12-10", "invalid", "EndDate")]
    public async Task SearchAdAnalytics_InvalidDateThrows(string startDate, string endDate, string expectedMessage)
    {
        var input = CreateInput();
        input.StartDate = startDate;
        input.EndDate = endDate;

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            LinkedIn.SearchAdAnalytics(input, CreateOptions(), default));

        Assert.That(exception.Message, Does.Contain(expectedMessage));
    }

    [Test]
    public async Task SearchAdAnalytics_ReturnsErrorWhenFailureThrowingIsDisabled()
    {
        var options = CreateOptions();
        options.ThrowErrorOnFailure = false;
        options.ErrorMessageOnFailure = "Invalid analytics input";
        var input = CreateInput();
        input.StartDate = "invalid";

        var result = await LinkedIn.SearchAdAnalytics(input, options, default);

        Assert.That(result.Success, Is.False);
    }

    [Test]
    public void SearchAdAnalytics_InvalidJsonThrows()
    {
        Handler.Response.Content = new StringContent("not-json");

        Assert.ThrowsAsync<Newtonsoft.Json.JsonReaderException>(() =>
            LinkedIn.SearchAdAnalytics(CreateInput(), CreateOptions(), default));
    }

    [Test]
    public void SearchAdAnalytics_EmptyResponseBodyReturnsEmptyValue()
    {
        Handler.Response.Content = new StringContent(string.Empty);

        var result = LinkedIn.SearchAdAnalytics(CreateInput(), CreateOptions(), default).GetAwaiter().GetResult();

        Assert.That(result.Body.ToString(), Is.Empty);
    }

    [Test]
    public void SearchAdAnalytics_CanceledTokenCancelsRequest()
    {
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.ThrowsAsync<OperationCanceledException>(() =>
            LinkedIn.SearchAdAnalytics(CreateInput(), CreateOptions(), cancellationSource.Token));
    }

    [Test]
    public void SearchAdAnalytics_HttpTimeoutIsReported()
    {
        Handler.ThrowTimeout = true;

        Assert.ThrowsAsync<ArgumentException>(() =>
            LinkedIn.SearchAdAnalytics(CreateInput(), CreateOptions(), default));
    }

    [Test]
    public void HttpClientFactory_AppliesOptionsAndDefaultHeaders()
    {
        var options = CreateOptions();
        using var client = new HttpClientFactory().CreateClient(options);
        client.SetDefaultRequestHeadersBasedOnOptions(options);

        Assert.That(client.Timeout, Is.EqualTo(TimeSpan.FromSeconds(options.ConnectionTimeoutSeconds)));
        Assert.That(client.DefaultRequestHeaders.GetValues("LinkedIn-Version"), Does.Contain(options.LinkedInAPIVersion));
        Assert.That(client.DefaultRequestHeaders.GetValues("X-Restli-Protocol-Version"), Does.Contain("2.0.0"));
    }

    private static Input CreateInput()
    {
        return new Input
        {
            Finder = Finder.Analytics,
            Pivot = Pivot.NONE,
            StartDate = "2023-12-10",
            Campaigns = Array.Empty<string>(),
            Creatives = Array.Empty<string>(),
            Metrics = Array.Empty<string>(),
            Pivots = Array.Empty<Pivot>(),
            TimeGranularity = TimeGranularity.ALL,
        };
    }

    private static Options CreateOptions()
    {
        return new Options
        {
            LinkedInAPIVersion = "202309",
            Token = "sample-access-token",
            ConnectionTimeoutSeconds = 37,
            AllowInvalidCertificate = true,
            AllowInvalidResponseContentTypeCharSet = true,
            AutomaticCookieHandling = true,
            FollowRedirects = true,
        };
    }

    private sealed class MockHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpMessageHandler handler;

        public MockHttpClientFactory(HttpMessageHandler handler)
        {
            this.handler = handler;
        }

        public HttpClient CreateClient(Options options)
        {
            return new HttpClient(this.handler, disposeHandler: false);
        }
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public HttpResponseMessage Response { get; set; }

        public Uri LastRequestUri { get; private set; }

        public string LastAuthorizationHeader { get; private set; }

        public bool ThrowTimeout { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.LastRequestUri = request.RequestUri;
            this.LastAuthorizationHeader = request.Headers.Authorization?.ToString();

            if (this.ThrowTimeout)
            {
                throw new TaskCanceledException();
            }

            return Task.FromResult(this.Response);
        }
    }
}
