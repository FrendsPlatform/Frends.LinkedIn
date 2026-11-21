using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Frends.LinkedIn.SearchAdAccountUser.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Frends.LinkedIn.SearchAdAccountUser.Tests;

[TestFixture]
[NonParallelizable]
internal sealed class UnitTests
{
    private static int nextTimeout = 30;
    private Connection connection;
    private StubHttpClientFactory clientFactory;
    private Input input;
    private Options options;

    [SetUp]
    public void Setup()
    {
        input = new Input
        {
            SearchMethod = SearchAttribute.FindAdAccountsByAuthenticatedUser,
        };
        connection = new Connection
        {
            Token = "test-token",
        };
        options = new Options
        {
            LinkedInAPIVersion = "202309",
            ConnectionTimeoutSeconds = Interlocked.Increment(ref nextTimeout),
        };
        clientFactory = new StubHttpClientFactory();
        LinkedIn.ClientFactory = clientFactory;
    }

    [Test]
    public async Task FindAdAccountsByAuthenticatedUser_ReturnsParsedResponseAndRequestHeaders()
    {
        var response = CreateResponse(HttpStatusCode.OK, "{\"elements\":[{\"id\":123}]}");
        response.Headers.Add("X-LinkedIn-Test", "header-value");
        clientFactory.Handler.Response = response;

        var result = await ExecuteAsync();

        Assert.That(result.Success, Is.True);
        Assert.That(result.Error, Is.Null);
        Assert.That(((JObject)result.Body)["elements"][0]["id"].Value<int>(), Is.EqualTo(123));
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Headers["X-LinkedIn-Test"], Is.EqualTo("header-value"));
        Assert.That(clientFactory.Handler.LastRequestUri, Is.EqualTo("https://api.linkedin.com/rest/adAccountUsers?q=authenticatedUser"));
        Assert.That(clientFactory.Handler.LastAuthorization, Does.StartWith("Bearer "));
        Assert.That(clientFactory.Handler.LastApiVersion, Is.EqualTo("202309"));
    }

    [Test]
    public async Task FindAdAccountUsersByAccounts_IncludesEachAccountUrn()
    {
        input.SearchMethod = SearchAttribute.FindAdAccountUsersByAccounts;
        input.AccountUrns =
        [
            new AccountUrn { Urn = "urn:li:sponsoredAccount:123" },
            new AccountUrn { Urn = "urn:li:sponsoredAccount:456" },
        ];
        clientFactory.Handler.Response = CreateResponse(HttpStatusCode.OK, "{\"elements\":[]}");

        var result = await ExecuteAsync();

        Assert.That(result.Success, Is.True);
        Assert.That(
            clientFactory.Handler.LastRequestUri,
            Is.EqualTo("https://api.linkedin.com/rest/adAccountUsers?q=accounts&accounts=urn:li:sponsoredAccount:123&accounts=urn:li:sponsoredAccount:456"));
    }

    [Test]
    public async Task GetAdAccountUser_UsesAccountAndUserUrns()
    {
        input.SearchMethod = SearchAttribute.GetAdAccountUser;
        input.AccountUrn = "urn:li:sponsoredAccount:123";
        input.UserUrn = "urn:li:person:abc";
        clientFactory.Handler.ResponseFactory = (_, _) =>
            Task.FromResult(CreateResponse(HttpStatusCode.OK, "{}"));

        var result = await ExecuteAsync();

        Assert.That(result.Success, Is.True);
        Assert.That(
            clientFactory.Handler.LastRequestUri,
            Is.EqualTo("https://api.linkedin.com/rest/adAccountUsers/account=urn:li:sponsoredAccount:123&user=urn:li:person:abc"));
    }

    [Test]
    public async Task EmptyResponseBody_IsReturnedAsAnEmptyJsonValue()
    {
        clientFactory.Handler.Response = CreateResponse(HttpStatusCode.OK, string.Empty);

        var result = await ExecuteAsync();

        Assert.That(result.Success, Is.True);
        Assert.That(((JValue)result.Body).Value, Is.EqualTo(string.Empty));
    }

    [Test]
    public async Task ErrorResponse_WhenThrowingIsDisabled_ReturnsErrorDetails()
    {
        options.ThrowErrorOnFailure = false;
        options.ErrorMessageOnFailure = "Custom search failure";
        clientFactory.Handler.Response = CreateResponse(HttpStatusCode.Forbidden, "{\"message\":\"Denied\"}");

        var result = await ExecuteAsync();

        Assert.That(result.Success, Is.False);
        Assert.That(result.StatusCode, Is.EqualTo(403));
    }

    [Test]
    public async Task InvalidResponseJson_WhenThrowingIsDisabled_ReturnsError()
    {
        options.ThrowErrorOnFailure = false;
        clientFactory.Handler.Response = CreateResponse(HttpStatusCode.OK, "not-json");

        var result = await ExecuteAsync();

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.AdditionalInfo, Is.TypeOf<JsonReaderException>());
    }

    [Test]
    public async Task MissingUrns_WhenThrowingIsDisabled_ReturnsInputError()
    {
        options.ThrowErrorOnFailure = false;
        input.SearchMethod = SearchAttribute.GetAdAccountUser;

        var result = await ExecuteAsync();

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.AdditionalInfo, Is.TypeOf<ArgumentException>());
        Assert.That(clientFactory.CreateCount, Is.Zero);
    }

    [Test]
    public async Task MissingAccountList_WhenThrowingIsDisabled_ReturnsInputError()
    {
        options.ThrowErrorOnFailure = false;
        input.SearchMethod = SearchAttribute.FindAdAccountUsersByAccounts;
        input.AccountUrns = null;

        var result = await ExecuteAsync();

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.AdditionalInfo, Is.TypeOf<ArgumentException>());
    }

    [Test]
    public async Task RequestTimeout_WhenThrowingIsDisabled_ReturnsTimeoutError()
    {
        options.ThrowErrorOnFailure = false;
        clientFactory.Handler.ResponseFactory = (_, _) =>
            Task.FromException<HttpResponseMessage>(new TaskCanceledException("request timed out"));

        var result = await ExecuteAsync();

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.AdditionalInfo, Is.TypeOf<ArgumentException>());
    }

    [Test]
    public void CanceledRequest_ThrowsCancellationException()
    {
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await LinkedIn.SearchAdAccountUser(input, connection, options, cancellationSource.Token));
        Assert.That(clientFactory.CreateCount, Is.Zero);
    }

    [Test]
    public async Task RepeatedRequestsWithSameOptions_ReuseHttpClient()
    {
        clientFactory.Handler.ResponseFactory = (_, _) =>
            Task.FromResult(CreateResponse(HttpStatusCode.OK, "{}"));

        await ExecuteAsync();
        await ExecuteAsync();

        Assert.That(clientFactory.CreateCount, Is.EqualTo(1));
    }

    [Test]
    public void HttpClientFactory_AppliesHandlerOptions()
    {
        using var client = new Frends.LinkedIn.SearchAdAccountUser.HttpClientFactory().CreateClient(new Options
        {
            FollowRedirects = false,
            AutomaticCookieHandling = false,
            AllowInvalidCertificate = true,
        });

        Assert.That(client, Is.Not.Null);
    }

    private static HttpResponseMessage CreateResponse(HttpStatusCode statusCode, string body)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };
    }

    private Task<Result> ExecuteAsync()
    {
        return LinkedIn.SearchAdAccountUser(input, connection, options, CancellationToken.None);
    }

    private sealed class StubHttpClientFactory : IHttpClientFactory
    {
        public StubHttpMessageHandler Handler { get; } = new();

        public int CreateCount { get; private set; }

        public HttpClient CreateClient(Options options)
        {
            CreateCount++;
            return new HttpClient(Handler);
        }
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> ResponseFactory { get; set; }

        public HttpResponseMessage Response { get; set; } = CreateResponse(HttpStatusCode.OK, "{}");

        public string LastApiVersion { get; private set; }

        public string LastAuthorization { get; private set; }

        public string LastRequestUri { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequestUri = request.RequestUri?.ToString();
            LastAuthorization = request.Headers.Authorization?.ToString();
            LastApiVersion = request.Headers.GetValues("LinkedIn-Version").Single();
            return ResponseFactory?.Invoke(request, cancellationToken) ?? Task.FromResult(Response);
        }
    }
}
