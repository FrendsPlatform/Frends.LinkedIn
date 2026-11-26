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
        var result = await LinkedIn.SearchAdAnalytics(CreateInput(), CreateConnection(), CreateOptions(), default);

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

        await LinkedIn.SearchAdAnalytics(input, CreateConnection(), CreateOptions(), default);

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

        await LinkedIn.SearchAdAnalytics(input, CreateConnection(), CreateOptions(), default);

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
            LinkedIn.SearchAdAnalytics(input, CreateConnection(), CreateOptions(), default));

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

        var result = await LinkedIn.SearchAdAnalytics(input, CreateConnection(), options, default);

        Assert.That(result.Success, Is.False);
    }

    [Test]
    public async Task SearchAdAnalytics_NonSuccessStatusCodeThrows()
    {
        Handler.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("{\"message\":\"bad\"}"),
        };

        var exception = await Assert.ThrowsAsync<HttpRequestException>(() =>
            LinkedIn.SearchAdAnalytics(CreateInput(), CreateConnection(), CreateOptions(), default));

        Assert.That(exception.Message, Does.Contain("400"));
    }

    [Test]
    public async Task SearchAdAnalytics_NonSuccessStatusCodeReturnsErrorWhenFailureThrowingIsDisabled()
    {
        Handler.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent("not-json"),
        };
        var options = CreateOptions();
        options.ThrowErrorOnFailure = false;

        var result = await LinkedIn.SearchAdAnalytics(CreateInput(), CreateConnection(), options, default);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.Contain("401"));
    }

    [Test]
    public void SearchAdAnalytics_InvalidJsonThrows()
    {
        Handler.Response.Content = new StringContent("not-json");

        Assert.ThrowsAsync<Newtonsoft.Json.JsonReaderException>(() =>
            LinkedIn.SearchAdAnalytics(CreateInput(), CreateConnection(), CreateOptions(), default));
    }

    [Test]
    public void SearchAdAnalytics_EmptyResponseBodyReturnsEmptyValue()
    {
        Handler.Response.Content = new StringContent(string.Empty);

        var result = LinkedIn.SearchAdAnalytics(CreateInput(), CreateConnection(), CreateOptions(), default).GetAwaiter().GetResult();

        Assert.That(result.Body.ToString(), Is.Empty);
    }

    [Test]
    public void SearchAdAnalytics_CanceledTokenCancelsRequest()
    {
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.ThrowsAsync<OperationCanceledException>(() =>
            LinkedIn.SearchAdAnalytics(CreateInput(), CreateConnection(), CreateOptions(), cancellationSource.Token));
    }

    [Test]
    public void SearchAdAnalytics_HttpTimeoutIsReported()
    {
        Handler.ThrowTimeout = true;

        Assert.ThrowsAsync<ArgumentException>(() =>
            LinkedIn.SearchAdAnalytics(CreateInput(), CreateConnection(), CreateOptions(), default));
    }

    [Test]
    public void HttpClientFactory_AppliesOptionsAndDefaultHeaders()
    {
        var connection = CreateConnection();
        using var client = new HttpClientFactory().CreateClient(connection);
        client.SetDefaultRequestHeadersBasedOnConnection(connection);

        Assert.That(client.Timeout, Is.EqualTo(TimeSpan.FromSeconds(connection.ConnectionTimeoutSeconds)));
        Assert.That(client.DefaultRequestHeaders.GetValues("LinkedIn-Version"), Does.Contain(connection.LinkedInAPIVersion));
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
        return new Options();
    }

    private static Connection CreateConnection()
    {
        return new Connection
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

        public HttpClient CreateClient(Connection connection)
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
