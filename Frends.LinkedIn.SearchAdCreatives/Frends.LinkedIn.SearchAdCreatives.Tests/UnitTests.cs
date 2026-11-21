using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Frends.LinkedIn.SearchAdCreatives;
using Frends.LinkedIn.SearchAdCreatives.Definitions;
using NUnit.Framework;

namespace Frends.LinkedIn.SearchAdCreatives.Tests;

/// <summary>
/// Tests for LinkedIn ad creative search behavior.
/// </summary>
[TestFixture]
internal class UnitTests
{
    private static int nextTimeout;
    private Input input;
    private Options options;
    private MockHttpClientFactory factory;

    /// <summary>
    /// Configures a mocked client factory and task parameters before each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        input = new Input
        {
            AdAccountId = "512500029",
            AdCampaignUrns = Array.Empty<AdCampaign>(),
        };
        options = new Options
        {
            LinkedInAPIVersion = "202309",
            ConnectionTimeoutSeconds = 30 + Interlocked.Increment(ref nextTimeout),
            ThrowExceptionOnErrorResponse = false,
            ThrowErrorOnFailure = false,
            AllowInvalidCertificate = false,
            AllowInvalidResponseContentTypeCharSet = false,
            AutomaticCookieHandling = true,
            FollowRedirects = true,
        };
        factory = new MockHttpClientFactory(
            (_, _) => Task.FromResult(CreateResponse(HttpStatusCode.OK, """{"elements":[]}""")));
        LinkedIn.ClientFactory = factory;
    }

    /// <summary>
    /// Verifies response data, headers, authentication, and list filter query parameters.
    /// </summary>
    [Test]
    public async Task SearchReturnsResponseAndUsesFilterParameters()
    {
        HttpRequestMessage capturedRequest = null;
        input.AdCampaignUrns =
        [
            new AdCampaign { AdCampaignUrn = "urn:li:sponsoredCampaign:360035215" },
        ];
        input.AdCreativesUrns =
        [
            new AdCreativeFilter { AdCreativeUrn = "urn:li:sponsoredCreative:119962155" },
        ];
        factory = new MockHttpClientFactory((request, _) =>
        {
            capturedRequest = request;
            var response = CreateResponse(HttpStatusCode.OK, """{"elements":[{"id":"119962155"}]}""");
            response.Headers.TryAddWithoutValidation("X-Request-Id", "abc123");
            return Task.FromResult(response);
        });
        LinkedIn.ClientFactory = factory;

        var result = await LinkedIn.SearchAdCreatives(input, CreateConnection(), options, default);

        Assert.That(result.Success, Is.True);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That((string)result.Body.elements[0].id, Is.EqualTo("119962155"));
        Assert.That(result.Headers["X-Request-Id"], Is.EqualTo("abc123"));
        Assert.That(result.Headers["Content-Type"], Does.StartWith("text/plain"));
        Assert.That(capturedRequest.Method, Is.EqualTo(HttpMethod.Get));
        Assert.That(capturedRequest.RequestUri.AbsoluteUri, Does.Contain("campaigns="));
        Assert.That(capturedRequest.RequestUri.AbsoluteUri, Does.Contain("creatives="));
        Assert.That(capturedRequest.Headers.Authorization.Parameter, Is.EqualTo("linkedin-access-token"));
    }

    /// <summary>
    /// Verifies that a single creative URN is added to the request path.
    /// </summary>
    [Test]
    public async Task SearchRequestsSingleCreativeByUrn()
    {
        HttpRequestMessage capturedRequest = null;
        input.GetSingleCreative = true;
        input.AdCreativeUrn = "urn:li:sponsoredCreative:119962155";
        factory = new MockHttpClientFactory((request, _) =>
        {
            capturedRequest = request;
            return Task.FromResult(CreateResponse(HttpStatusCode.OK, string.Empty));
        });
        LinkedIn.ClientFactory = factory;

        var result = await LinkedIn.SearchAdCreatives(input, CreateConnection(), options, default);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Body.ToString(), Is.Empty);
        Assert.That(
            Uri.UnescapeDataString(capturedRequest.RequestUri.AbsoluteUri),
            Does.Contain("urn:li:sponsoredCreative:119962155"));
    }

    /// <summary>
    /// Verifies that a configured unsuccessful HTTP response becomes a failure result.
    /// </summary>
    [Test]
    public async Task SearchReturnsFailureForUnsuccessfulResponseWhenConfigured()
    {
        options.ThrowExceptionOnErrorResponse = true;
        factory = new MockHttpClientFactory(
            (_, _) => Task.FromResult(CreateResponse(HttpStatusCode.NotFound, """{"message":"missing"}""")));
        LinkedIn.ClientFactory = factory;

        var result = await LinkedIn.SearchAdCreatives(input, CreateConnection(), options, default);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.Contain("status code 404"));
        Assert.That(result.Error.AdditionalInfo, Is.TypeOf<WebException>());
    }

    /// <summary>
    /// Verifies that malformed JSON is reported through the task error result.
    /// </summary>
    [Test]
    public async Task SearchReturnsFailureForMalformedJson()
    {
        factory = new MockHttpClientFactory(
            (_, _) => Task.FromResult(CreateResponse(HttpStatusCode.OK, "not json")));
        LinkedIn.ClientFactory = factory;

        var result = await LinkedIn.SearchAdCreatives(input, CreateConnection(), options, default);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.Contain("Unable to read response message as json"));
    }

    /// <summary>
    /// Verifies that timeout cancellation is returned as a task failure.
    /// </summary>
    [Test]
    public async Task SearchReturnsFailureForRequestTimeout()
    {
        factory = new MockHttpClientFactory(
            (_, _) => Task.FromException<HttpResponseMessage>(new TaskCanceledException("request timed out")));
        LinkedIn.ClientFactory = factory;

        var result = await LinkedIn.SearchAdCreatives(input, CreateConnection(), options, default);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.AdditionalInfo, Is.TypeOf<ArgumentException>());
    }

    /// <summary>
    /// Verifies that configured errors are thrown with the custom message.
    /// </summary>
    [Test]
    public void SearchThrowsConfiguredError()
    {
        options.ThrowErrorOnFailure = true;
        options.ErrorMessageOnFailure = "Creative search failed";
        factory = new MockHttpClientFactory(
            (_, _) => Task.FromResult(CreateResponse(HttpStatusCode.OK, "not json")));
        LinkedIn.ClientFactory = factory;

        var exception = Assert.ThrowsAsync<Exception>(
            async () => await LinkedIn.SearchAdCreatives(input, CreateConnection(), options, default));

        Assert.That(exception.Message, Is.EqualTo("Creative search failed"));
    }

    /// <summary>
    /// Verifies that cancellation requested by Frends is propagated.
    /// </summary>
    [Test]
    public void SearchPropagatesCancellation()
    {
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.ThrowsAsync<OperationCanceledException>(
            async () => await LinkedIn.SearchAdCreatives(
                input,
                CreateConnection(),
                options,
                cancellationSource.Token));
    }

    /// <summary>
    /// Verifies that matching options reuse the cached client.
    /// </summary>
    [Test]
    public async Task SearchReusesClientForMatchingOptions()
    {
        await LinkedIn.SearchAdCreatives(input, CreateConnection(), options, default);
        await LinkedIn.SearchAdCreatives(input, CreateConnection(), options, default);

        Assert.That(factory.CreateCount, Is.EqualTo(1));
        Assert.That(factory.RequestCount, Is.EqualTo(2));
    }

    /// <summary>
    /// Verifies client headers and handler settings are configured from task options.
    /// </summary>
    [Test]
    public void HttpClientConfigurationUsesOptions()
    {
        using var handler = new HttpClientHandler();
        options.AllowInvalidCertificate = true;
        options.AutomaticCookieHandling = false;
        options.FollowRedirects = false;
        handler.SetHandlerSettingsBasedOnOptions(options);

        Assert.That(handler.AllowAutoRedirect, Is.False);
        Assert.That(handler.UseCookies, Is.False);
        Assert.That(handler.ServerCertificateCustomValidationCallback, Is.Not.Null);

        using var client = new HttpClientFactory().CreateClient(options);
        client.SetDefaultRequestHeadersBasedOnOptions(options, getSingleCreative: false);

        Assert.That(client.Timeout, Is.EqualTo(TimeSpan.FromSeconds(options.ConnectionTimeoutSeconds)));
        Assert.That(client.DefaultRequestHeaders.GetValues("LinkedIn-Version"), Does.Contain("202309"));
        Assert.That(client.DefaultRequestHeaders.GetValues("X-RestLi-Method"), Does.Contain("FINDER"));
    }

    private static Connection CreateConnection()
    {
        return new Connection { Token = "linkedin-access-token" };
    }

    private static HttpResponseMessage CreateResponse(HttpStatusCode statusCode, string content)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content),
        };
    }

    private sealed class MockHttpClientFactory : IHttpClientFactory
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync;

        internal MockHttpClientFactory(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync)
        {
            this.sendAsync = sendAsync;
        }

        internal int CreateCount { get; private set; }

        internal int RequestCount { get; private set; }

        public HttpClient CreateClient(Options options)
        {
            CreateCount++;
            var handler = new MockHttpMessageHandler(sendAsync, () => RequestCount++);
            return new HttpClient(handler);
        }
    }

    private sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync;
        private readonly Action onRequest;

        internal MockHttpMessageHandler(
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync,
            Action onRequest)
        {
            this.sendAsync = sendAsync;
            this.onRequest = onRequest;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            onRequest();
            return sendAsync(request, cancellationToken);
        }
    }
}
