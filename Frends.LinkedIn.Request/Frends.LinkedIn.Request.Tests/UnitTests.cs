using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Frends.LinkedIn.Request.Definitions;
using NUnit.Framework;

namespace Frends.LinkedIn.Request.Tests;

[TestFixture]
internal class UnitTests
{
    [Test]
    public async Task RequestReturnsParsedResponseAndSendsHeaders()
    {
        using var handler = new MockHttpMessageHandler(HttpStatusCode.OK, "{\"value\":42}");
        using var client = new HttpClient(handler);
        var input = new Input
        {
            Method = Method.GET,
            Url = "https://api.linkedin.test/rest/resource",
            Headers = new[]
            {
                new Header
                {
                    Name = "X-Custom",
                    Value = "custom-value",
                },
            },
        };
        var options = DefaultOptions();

        var result = await LinkedIn.RequestWithHttpClient(input, options, CancellationToken.None, client);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Error, Is.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Body.ToString(), Does.Contain("\"value\": 42"));
        Assert.That(handler.Method, Is.EqualTo("GET"));
        Assert.That(handler.RequestUri, Is.EqualTo(input.Url));
        Assert.That(handler.Authorization, Is.EqualTo("Bearer test-token"));
        Assert.That(handler.RequestHeaders["X-Custom"], Is.EqualTo("custom-value"));
        Assert.That(result.Headers["X-Response"], Is.EqualTo("response-value"));
        Assert.That(result.Headers["Content-Type"], Does.Contain("application/json"));
        Assert.That(handler.RequestBody, Is.Null);
    }

    [Test]
    public async Task RequestSendsMessageForMethodsThatAllowContent()
    {
        using var handler = new MockHttpMessageHandler(HttpStatusCode.OK, "\"accepted\"");
        using var client = new HttpClient(handler);
        var input = new Input
        {
            Method = Method.POST,
            Url = "https://api.linkedin.test/rest/resource",
            Message = "{\"name\":\"sample\"}",
            Headers = new[]
            {
                new Header
                {
                    Name = "Content-Type",
                    Value = "application/json; charset=utf-8",
                },
                new Header
                {
                    Name = "X-Custom",
                    Value = "custom-value",
                },
            },
        };

        var result = await LinkedIn.RequestWithHttpClient(input, DefaultOptions(), CancellationToken.None, client);

        Assert.That(result.Success, Is.True);
        Assert.That(handler.Method, Is.EqualTo("POST"));
        Assert.That(handler.RequestBody, Is.EqualTo(input.Message));
        Assert.That(handler.ContentType, Does.Contain("application/json"));
        Assert.That(handler.RequestHeaders["X-Custom"], Is.EqualTo("custom-value"));
    }

    [Test]
    public async Task RequestReturnsEmptyStringForEmptyResponseBody()
    {
        using var handler = new MockHttpMessageHandler(HttpStatusCode.NoContent, "  ");
        using var client = new HttpClient(handler);

        var result = await LinkedIn.RequestWithHttpClient(
            DefaultInput(),
            DefaultOptions(),
            CancellationToken.None,
            client);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Body.ToString(), Is.Empty);
    }

    [Test]
    public async Task RequestReturnsFailureForInvalidJsonWhenConfiguredNotToThrow()
    {
        using var handler = new MockHttpMessageHandler(HttpStatusCode.OK, "not-json");
        using var client = new HttpClient(handler);
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;

        var result = await LinkedIn.RequestWithHttpClient(DefaultInput(), options, CancellationToken.None, client);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.Contain("Unable to read response message as json"));
        Assert.That(result.Error.AdditionalInfo, Is.TypeOf<Newtonsoft.Json.JsonReaderException>());
    }

    [Test]
    public async Task RequestIncludesCustomErrorMessageForHttpFailure()
    {
        using var handler = new MockHttpMessageHandler(HttpStatusCode.Forbidden, "{\"error\":\"denied\"}");
        using var client = new HttpClient(handler);
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;
        options.ErrorMessageOnFailure = "LinkedIn request failed";

        var result = await LinkedIn.RequestWithHttpClient(DefaultInput(), options, CancellationToken.None, client);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.StartWith("LinkedIn request failed:"));
        Assert.That(result.Error.Message, Does.Contain("status code 403"));
        Assert.That(result.Error.AdditionalInfo, Is.TypeOf<WebException>());
    }

    [Test]
    public async Task RequestReturnsHttpErrorResponseWhenConfiguredNotToThrow()
    {
        using var handler = new MockHttpMessageHandler(HttpStatusCode.BadRequest, "{\"error\":\"bad request\"}");
        using var client = new HttpClient(handler);
        var options = DefaultOptions();
        options.ThrowExceptionOnErrorResponse = false;

        var result = await LinkedIn.RequestWithHttpClient(DefaultInput(), options, CancellationToken.None, client);

        Assert.That(result.Success, Is.True);
        Assert.That(result.StatusCode, Is.EqualTo(400));
        Assert.That(result.Error, Is.Null);
    }

    [Test]
    public void RequestPropagatesCancellation()
    {
        using var handler = new MockHttpMessageHandler(HttpStatusCode.OK, "{}");
        using var client = new HttpClient(handler);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await LinkedIn.RequestWithHttpClient(DefaultInput(), DefaultOptions(), cancellation.Token, client));
        Assert.That(handler.WasCalled, Is.False);
    }

    [Test]
    public async Task RequestConvertsUnexpectedHttpCancellationToTimeoutFailure()
    {
        using var handler = new MockHttpMessageHandler(new TaskCanceledException("request timed out"));
        using var client = new HttpClient(handler);
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;

        var result = await LinkedIn.RequestWithHttpClient(DefaultInput(), options, CancellationToken.None, client);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Does.Contain("most likely due to a timeout"));
        Assert.That(result.Error.AdditionalInfo, Is.TypeOf<ArgumentException>());
    }

    [Test]
    public async Task RequestHandlesInvalidUrlAsFailure()
    {
        using var handler = new MockHttpMessageHandler(HttpStatusCode.OK, "{}");
        using var client = new HttpClient(handler);
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;
        var input = DefaultInput();
        input.Url = "not-an-absolute-url";

        var result = await LinkedIn.RequestWithHttpClient(input, options, CancellationToken.None, client);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(handler.WasCalled, Is.False);
    }

    [Test]
    public async Task RequestHandlesDuplicateHeadersAsFailure()
    {
        using var handler = new MockHttpMessageHandler(HttpStatusCode.OK, "{}");
        using var client = new HttpClient(handler);
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;
        var input = DefaultInput();
        input.Headers = new[]
        {
            new Header
            {
                Name = "X-Duplicate",
                Value = "one",
            },
            new Header
            {
                Name = "x-duplicate",
                Value = "two",
            },
        };

        var result = await LinkedIn.RequestWithHttpClient(input, options, CancellationToken.None, client);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.AdditionalInfo, Is.TypeOf<ArgumentException>());
        Assert.That(handler.WasCalled, Is.False);
    }

    [Test]
    public void HttpClientFactoryCreatesConfiguredClientAndRequestHeaders()
    {
        var options = DefaultOptions();
        options.ConnectionTimeoutSeconds = 46;
        var factory = new HttpClientFactory();

        using var client = factory.CreateClient(options);
        client.SetDefaultRequestHeadersBasedOnOptions(options);

        Assert.That(client.Timeout, Is.EqualTo(TimeSpan.FromSeconds(46)));
        Assert.That(client.DefaultRequestHeaders.ExpectContinue, Is.False);
        Assert.That(client.DefaultRequestHeaders.GetValues("LinkedIn-Version"), Is.EqualTo(new[] { "202309" }));
        Assert.That(client.DefaultRequestHeaders.GetValues("X-Restli-Protocol-Version"), Is.EqualTo(new[] { "2.0.0" }));
    }

    [Test]
    public void RequestClientCacheReusesClientForEquivalentOptions()
    {
        var options = DefaultOptions();
        options.ConnectionTimeoutSeconds = 47;

        var firstClient = LinkedIn.GetHttpClientForOptions(options);
        var secondClient = LinkedIn.GetHttpClientForOptions(options);

        Assert.That(secondClient, Is.SameAs(firstClient));
        Assert.That(firstClient.Timeout, Is.EqualTo(TimeSpan.FromSeconds(47)));
    }

    [Test]
    public void ErrorHandlerReturnsFailureWithOriginalException()
    {
        var exception = new InvalidOperationException("Request failed.");
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;
        options.ErrorMessageOnFailure = "Custom message";

        var result = exception.Handle(options);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error.Message, Is.EqualTo("Custom message: Request failed."));
        Assert.That(result.Error.AdditionalInfo, Is.SameAs(exception));
    }

    [Test]
    public void ErrorHandlerRethrowsOriginalExceptionWhenConfigured()
    {
        var exception = new InvalidOperationException("Request failed.");

        var actual = Assert.Throws<InvalidOperationException>(() => exception.Handle(DefaultOptions()));

        Assert.That(actual, Is.SameAs(exception));
    }

    [Test]
    public void ErrorHandlerPreservesCancellation()
    {
        var exception = new OperationCanceledException("Cancelled.");
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;

        var actual = Assert.Throws<OperationCanceledException>(() => exception.Handle(options));

        Assert.That(actual, Is.SameAs(exception));
    }

    [Test]
    public void ErrorHandlerRejectsNullArguments()
    {
        Assert.Throws<ArgumentNullException>(() => ((Exception)null).Handle(DefaultOptions()));
        Assert.Throws<ArgumentNullException>(() => new Exception().Handle(null));
    }

    private static Input DefaultInput()
    {
        return new Input
        {
            Method = Method.GET,
            Url = "https://api.linkedin.test/rest/resource",
            Headers = Array.Empty<Header>(),
        };
    }

    private static Options DefaultOptions()
    {
        return new Options
        {
            LinkedInAPIVersion = "202309",
            Token = "test-token",
            ThrowErrorOnFailure = true,
            ThrowExceptionOnErrorResponse = true,
            ConnectionTimeoutSeconds = 30,
            FollowRedirects = true,
            AutomaticCookieHandling = true,
        };
    }

    private sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode statusCode;
        private readonly string responseBody;
        private readonly Exception exception;

        internal MockHttpMessageHandler(HttpStatusCode statusCode, string responseBody)
        {
            this.statusCode = statusCode;
            this.responseBody = responseBody;
        }

        internal MockHttpMessageHandler(Exception exception)
        {
            this.exception = exception;
        }

        internal bool WasCalled { get; private set; }

        internal string Method { get; private set; }

        internal string RequestUri { get; private set; }

        internal string Authorization { get; private set; }

        internal string RequestBody { get; private set; }

        internal string ContentType { get; private set; }

        internal System.Collections.Generic.Dictionary<string, string> RequestHeaders { get; } =
            new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            this.WasCalled = true;
            this.Method = request.Method.Method;
            this.RequestUri = request.RequestUri.ToString();
            this.Authorization = request.Headers.Authorization?.ToString();
            foreach (var header in request.Headers)
            {
                this.RequestHeaders[header.Key] = string.Join(",", header.Value);
            }

            if (request.Content != null)
            {
                this.RequestBody = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                this.ContentType = request.Content.Headers.ContentType?.ToString();
            }

            if (this.exception != null)
            {
                throw this.exception;
            }

            var response = new HttpResponseMessage(this.statusCode)
            {
                Content = new StringContent(this.responseBody, Encoding.UTF8, "application/json"),
            };
            response.Headers.TryAddWithoutValidation("X-Response", "response-value");
            return response;
        }
    }
}
