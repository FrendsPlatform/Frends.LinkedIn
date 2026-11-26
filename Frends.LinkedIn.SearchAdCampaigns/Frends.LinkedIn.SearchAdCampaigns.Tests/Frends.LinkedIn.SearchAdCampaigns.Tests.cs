using System.Net;
using System.Text;
using Frends.LinkedIn.SearchAdCampaigns.Definitions;
using NUnit.Framework;

namespace Frends.LinkedIn.SearchAdCampaigns.Tests;

[TestFixture]
public class UnitTests
{
    private Input input;
    private Connection connection;
    private Options options;
    private MockHandler handler;
    private IHttpClientFactory originalFactory;

    [SetUp]
    public void Setup()
    {
        input = new Input
        {
            AdAccountId = "512500029",
            Id = string.Empty,
            Name = string.Empty,
            Reference = string.Empty,
            TypeFilters = null,
            StatusFilters = null,
        };

        connection = new Connection
        {
            Token = "token",
            LinkedInAPIVersion = "202309",
            ConnectionTimeoutSeconds = 30,
            AllowInvalidCertificate = true,
            AutomaticCookieHandling = true,
            FollowRedirects = true,
        };

        options = new Options { ThrowErrorOnFailure = true };

        handler = new MockHandler();
        originalFactory = LinkedIn.ClientFactory;
        LinkedIn.ClientFactory = new MockFactory(handler);
    }

    [TearDown]
    public void TearDown()
    {
        LinkedIn.ClientFactory = originalFactory;
    }

    [Test]
    public async Task GetAllCampaigns()
    {
        var result = await LinkedIn.SearchAdCampaigns(input, connection, options, default);

        Assert.That(result.Success, Is.True);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Error, Is.Null);
        Assert.That((int)result.Body.paging.total, Is.EqualTo(1));
        Assert.That(result.Headers["X-Test"], Is.EqualTo("a;b"));
        Assert.That(handler.Request.RequestUri.OriginalString, Is.EqualTo("https://api.linkedin.com/rest/adAccounts/512500029/adCampaigns?q=search&search=()"));
        Assert.That(handler.Request.Headers.Authorization.Scheme, Is.EqualTo("Bearer"));
        Assert.That(handler.Request.Headers.Authorization.Parameter, Is.EqualTo(connection.Token));
        Assert.That(handler.Request.Headers.GetValues("LinkedIn-Version").Single(), Is.EqualTo("202309"));
    }

    [Test]
    public async Task GetCampaignsWithAllFilters()
    {
        input.Id = "277937223";
        input.Name = "Test campaign";
        input.Reference = "urn:li:organization:1";
        input.TypeFilters = new[] { new TypeFilter { CampaignType = CampaignType.TEXT_AD }, new TypeFilter { CampaignType = CampaignType.DYNAMIC } };
        input.StatusFilters = new[] { new StatusFilter { CampaignStatus = CampaignStatus.ACTIVE } };

        var result = await LinkedIn.SearchAdCampaigns(input, connection, options, default);

        Assert.That(result.Success, Is.True);
        Assert.That(
            handler.Request.RequestUri.OriginalString,
            Is.EqualTo("https://api.linkedin.com/rest/adAccounts/512500029/adCampaigns?q=search&search=(id:(values:List(277937223)),name:(values:List(Test campaign)),reference:(values:List(urn:li:organization:1)),type:(values:List(TEXT_AD,DYNAMIC)),status:(values:List(ACTIVE)))"));
    }

    [Test]
    public async Task EmptyResponseBodyGivesEmptyString()
    {
        handler.Content = string.Empty;

        var result = await LinkedIn.SearchAdCampaigns(input, connection, options, default);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Body.ToString(), Is.Empty);
    }

    [Test]
    public async Task UnsuccessfulStatusThrowsWhenThrowErrorOnFailure()
    {
        handler.Status = HttpStatusCode.Unauthorized;

        var ex = await Assert.ThrowsAsync<WebException>(() => LinkedIn.SearchAdCampaigns(input, connection, options, default));

        Assert.That(ex.Message, Does.Contain("401"));
    }

    [Test]
    public async Task CustomErrorMessageIsUsed()
    {
        handler.Status = HttpStatusCode.Unauthorized;
        options.ErrorMessageOnFailure = "Search failed";

        var ex = await Assert.ThrowsAsync<Exception>(() => LinkedIn.SearchAdCampaigns(input, connection, options, default));

        Assert.That(ex.Message, Is.EqualTo("Search failed"));
        Assert.That(ex.InnerException, Is.TypeOf<WebException>());
    }

    [Test]
    public async Task UnsuccessfulStatusReturnsErrorResultWhenNotThrowing()
    {
        handler.Status = HttpStatusCode.Forbidden;
        options.ThrowErrorOnFailure = false;
        options.ErrorMessageOnFailure = "Search failed";

        var result = await LinkedIn.SearchAdCampaigns(input, connection, options, default);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.StartWith("Search failed: "));
        Assert.That(result.Error.Message, Does.Contain("403"));
        Assert.That(result.Error.AdditionalInfo, Is.TypeOf<WebException>());
    }

    [Test]
    public async Task InvalidJsonBodyReturnsErrorResultWhenNotThrowing()
    {
        handler.Content = "not json";
        options.ThrowErrorOnFailure = false;

        var result = await LinkedIn.SearchAdCampaigns(input, connection, options, default);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.Contain("Unable to read response message as json"));
    }

    [Test]
    public async Task CanceledTokenThrows()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        options.ThrowErrorOnFailure = false;

        await Assert.ThrowsAsync<OperationCanceledException>(() => LinkedIn.SearchAdCampaigns(input, connection, options, cts.Token));
    }

    [Test]
    public async Task TimeoutIsReportedAsError()
    {
        handler.Exception = new TaskCanceledException();
        options.ThrowErrorOnFailure = false;

        var result = await LinkedIn.SearchAdCampaigns(input, connection, options, default);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.Contain("timeout"));
    }

    [Test]
    public async Task CancellationDuringRequestIsRethrown()
    {
        using var cts = new CancellationTokenSource();
        handler.Exception = new TaskCanceledException();
        handler.OnSend = cts.Cancel;

        await Assert.ThrowsAsync<TaskCanceledException>(() => LinkedIn.SearchAdCampaigns(input, connection, options, cts.Token));
    }

    [Test]
    public void DefaultFactoryAppliesOptions()
    {
        connection.ConnectionTimeoutSeconds = 5;
        connection.AutomaticCookieHandling = false;
        connection.FollowRedirects = false;

        using var client = new HttpClientFactory().CreateClient(connection);
        client.SetDefaultRequestHeadersBasedOnConnection(connection);

        Assert.That(client.Timeout, Is.EqualTo(TimeSpan.FromSeconds(5)));
        Assert.That(client.DefaultRequestHeaders.GetValues("X-Restli-Protocol-Version").Single(), Is.EqualTo("2.0.0"));
    }

    private sealed class MockFactory : IHttpClientFactory
    {
        private readonly HttpMessageHandler messageHandler;

        public MockFactory(HttpMessageHandler messageHandler) => this.messageHandler = messageHandler;

        public HttpClient CreateClient(Connection connection) => new(messageHandler, false);
    }

    private sealed class MockHandler : HttpMessageHandler
    {
        public HttpRequestMessage Request { get; private set; }

        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

        public string Content { get; set; } = "{\"paging\":{\"start\":0,\"count\":10,\"total\":1},\"elements\":[]}";

        public Exception Exception { get; set; }

        public Action OnSend { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            OnSend?.Invoke();

            if (Exception != null)
                throw Exception;

            var response = new HttpResponseMessage(Status)
            {
                Content = new StringContent(Content, Encoding.UTF8, "application/json"),
            };
            response.Headers.Add("X-Test", new[] { "a", "b" });
            return Task.FromResult(response);
        }
    }
}
