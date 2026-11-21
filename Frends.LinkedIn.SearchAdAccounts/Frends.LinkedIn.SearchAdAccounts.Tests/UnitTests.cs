global using System.Net;
global using System.Text;
global using Frends.LinkedIn.SearchAdAccounts.Definitions;
global using Newtonsoft.Json;
global using NUnit.Framework;
global using NUnit.Framework.Legacy;
global using LinkedInHttpClientFactory = Frends.LinkedIn.SearchAdAccounts.Definitions.IHttpClientFactory;
using Frends.LinkedIn.SearchAdAccounts.Helpers;

namespace Frends.LinkedIn.SearchAdAccounts.Tests;

/// <summary>
/// Unit tests for LinkedIn ad account searches.
/// </summary>
[TestFixture]
internal class UnitTests
{
    private static int nextTimeout = 100;
    private Options options;
    private MockHttpMessageHandler handler;

    /// <summary>
    /// Configures an isolated mock HTTP client for each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.handler = new MockHttpMessageHandler();
        LinkedIn.ClientFactory = new MockHttpClientFactory(this.handler);
        this.options = new Options
        {
            LinkedInAPIVersion = "202309",
            Token = "test-token",
            ThrowExceptionOnErrorResponse = true,
            ConnectionTimeoutSeconds = Interlocked.Increment(ref nextTimeout),
            ThrowErrorOnFailure = false,
        };
    }

    /// <summary>
    /// Verifies search filters, request headers, response body, and response headers.
    /// </summary>
    /// <returns>A task that completes when the response assertions finish.</returns>
    [Test]
    public async Task SearchAdAccounts_SendsFiltersAndReturnsResponse()
    {
        this.handler.ResponseFactory = (_, _) =>
        {
            var response = CreateResponse(HttpStatusCode.OK, """{"elements":[{"id":"512500029"}]}""");
            response.Headers.Add("x-request-id", "request-123");
            return Task.FromResult(response);
        };
        var input = new Input
        {
            Id = "512500029",
            Name = "Frends Marketing",
            Reference = "urn:li:organization:123456",
            TypeFilters = new[] { new TypeFilter { AccountType = AccountType.BUSINESS } },
            StatusFilters = new[] { new StatusFilter { AccountStatus = AccountStatus.ACTIVE } },
        };

        var result = await LinkedIn.SearchAdAccounts(input, this.options, CancellationToken.None);

        ClassicAssert.IsTrue(result.Success);
        ClassicAssert.AreEqual(200, result.StatusCode);
        ClassicAssert.AreEqual("512500029", (string)result.Body["elements"][0]["id"]);
        ClassicAssert.AreEqual(
            "request-123",
            result.Headers.Single(header => header.Key.Equals("x-request-id", StringComparison.OrdinalIgnoreCase)).Value);
        ClassicAssert.AreEqual("application/json; charset=utf-8", result.Headers["Content-Type"]);
        ClassicAssert.AreEqual(
            "https://api.linkedin.com/rest/adAccounts?q=search&search=(id:(values:List(512500029)),name:(values:List(Frends Marketing)),reference:(values:List(urn:li:organization:123456)),type:(values:List(BUSINESS)),status:(values:List(ACTIVE)))",
            this.handler.LastRequestUri);
        ClassicAssert.That(this.handler.LastAuthorization, Does.StartWith("Bearer "));
        ClassicAssert.AreEqual("202309", this.handler.LastLinkedInVersion);
    }

    /// <summary>
    /// Verifies that an empty response body is represented by an empty JSON value.
    /// </summary>
    /// <returns>A task that completes when the response assertions finish.</returns>
    [Test]
    public async Task SearchAdAccounts_EmptyResponseBodyReturnsSuccess()
    {
        this.handler.ResponseFactory = (_, _) => Task.FromResult(CreateResponse(HttpStatusCode.NoContent, string.Empty));
        this.options.ThrowExceptionOnErrorResponse = false;

        var result = await LinkedIn.SearchAdAccounts(new Input(), this.options, CancellationToken.None);

        ClassicAssert.IsTrue(result.Success);
        ClassicAssert.AreEqual(204, result.StatusCode);
        ClassicAssert.AreEqual(string.Empty, (string)result.Body);
    }

    /// <summary>
    /// Verifies that a failed HTTP response is returned through the standardized error result.
    /// </summary>
    /// <returns>A task that completes when the failure result assertions finish.</returns>
    [Test]
    public async Task SearchAdAccounts_HttpFailureReturnsErrorWhenConfigured()
    {
        this.handler.ResponseFactory = (_, _) =>
            Task.FromResult(CreateResponse(HttpStatusCode.BadRequest, """{"message":"invalid request"}"""));
        this.options.ErrorMessageOnFailure = "Search failed";

        var result = await LinkedIn.SearchAdAccounts(new Input(), this.options, CancellationToken.None);

        ClassicAssert.IsFalse(result.Success);
        ClassicAssert.IsInstanceOf<WebException>(result.Error.AdditionalInfo);
        ClassicAssert.That(result.Error.Message, Does.StartWith("Search failed:"));
    }

    /// <summary>
    /// Verifies that failures are thrown when configured.
    /// </summary>
    [Test]
    public void SearchAdAccounts_ThrowsWhenFailureIsConfiguredToThrow()
    {
        this.handler.ResponseFactory = (_, _) =>
            Task.FromResult(CreateResponse(HttpStatusCode.BadRequest, """{"message":"invalid request"}"""));
        this.options.ThrowErrorOnFailure = true;

        Assert.ThrowsAsync<WebException>(async () =>
            await LinkedIn.SearchAdAccounts(new Input(), this.options, CancellationToken.None));
    }

    /// <summary>
    /// Verifies that malformed JSON is returned as a task failure.
    /// </summary>
    /// <returns>A task that completes when the failure result assertions finish.</returns>
    [Test]
    public async Task SearchAdAccounts_InvalidJsonReturnsError()
    {
        this.handler.ResponseFactory = (_, _) => Task.FromResult(CreateResponse(HttpStatusCode.OK, "not json"));

        var result = await LinkedIn.SearchAdAccounts(new Input(), this.options, CancellationToken.None);

        ClassicAssert.IsFalse(result.Success);
        ClassicAssert.IsInstanceOf<JsonReaderException>(result.Error.AdditionalInfo);
    }

    /// <summary>
    /// Verifies that caller cancellation is propagated.
    /// </summary>
    [Test]
    public void SearchAdAccounts_PropagatesCancellation()
    {
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await LinkedIn.SearchAdAccounts(new Input(), this.options, cancellationSource.Token));
    }

    /// <summary>
    /// Verifies that request timeouts are reported as task failures.
    /// </summary>
    /// <returns>A task that completes when the failure result assertions finish.</returns>
    [Test]
    public async Task SearchAdAccounts_TimeoutReturnsError()
    {
        this.handler.ResponseFactory = (_, _) => Task.FromException<HttpResponseMessage>(new TaskCanceledException());

        var result = await LinkedIn.SearchAdAccounts(new Input(), this.options, CancellationToken.None);

        ClassicAssert.IsFalse(result.Success);
        ClassicAssert.IsInstanceOf<ArgumentException>(result.Error.AdditionalInfo);
    }

    /// <summary>
    /// Verifies the configured HTTP client settings and request header defaults.
    /// </summary>
    [Test]
    public void HttpClientFactory_AppliesOptions()
    {
        var options = new Options
        {
            AllowInvalidCertificate = true,
            AutomaticCookieHandling = false,
            ConnectionTimeoutSeconds = 45,
            FollowRedirects = false,
            LinkedInAPIVersion = "202501",
        };

        using var handler = new HttpClientHandler();
        handler.SetHandlerSettingsBasedOnOptions(options);
        using var client = new HttpClientFactory().CreateClient(options);
        client.SetDefaultRequestHeadersBasedOnOptions(options);

        ClassicAssert.IsFalse(handler.AllowAutoRedirect);
        ClassicAssert.IsFalse(handler.UseCookies);
        ClassicAssert.IsNotNull(handler.ServerCertificateCustomValidationCallback);
        ClassicAssert.AreEqual(TimeSpan.FromSeconds(45), client.Timeout);
        ClassicAssert.IsTrue(client.DefaultRequestHeaders.Contains("LinkedIn-Version"));
        ClassicAssert.IsTrue(client.DefaultRequestHeaders.Contains("X-Restli-Protocol-Version"));
    }

    private static HttpResponseMessage CreateResponse(HttpStatusCode statusCode, string body)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };
    }

    private sealed class MockHttpClientFactory : LinkedInHttpClientFactory
    {
        private readonly HttpMessageHandler innerHandler;

        public MockHttpClientFactory(HttpMessageHandler handler)
        {
            this.innerHandler = handler;
        }

        public HttpClient CreateClient(Options options) => new(this.innerHandler, disposeHandler: false);
    }

    private sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> ResponseFactory { get; set; }

        public string LastRequestUri { get; private set; }

        public string LastAuthorization { get; private set; }

        public string LastLinkedInVersion { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            this.LastRequestUri = request.RequestUri.ToString();
            this.LastAuthorization = request.Headers.Authorization.ToString();
            this.LastLinkedInVersion = request.Headers.GetValues("LinkedIn-Version").Single();
            return this.ResponseFactory(request, cancellationToken);
        }
    }
}
