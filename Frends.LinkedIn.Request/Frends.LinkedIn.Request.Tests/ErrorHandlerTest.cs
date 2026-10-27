namespace Frends.LinkedIn.Request.Tests;

using System;
using System.Threading;
using System.Threading.Tasks;
using Frends.LinkedIn.Request.Definitions;
using NUnit.Framework;

[TestFixture]
internal class ErrorHandlerTest
{
    private const string CustomErrorMessage = "LinkedIn request failed";

    [Test]
    public void Should_Throw_Error_When_ThrowErrorOnFailure_Is_True()
    {
        var exception = Assert.ThrowsAsync<UriFormatException>(() =>
            LinkedIn.Request(DefaultInput(), DefaultOptions(), CancellationToken.None));

        Assert.That(exception, Is.Not.Null);
    }

    [Test]
    public async Task Should_Return_Failed_Result_When_ThrowErrorOnFailure_Is_False()
    {
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;

        var result = await LinkedIn.Request(DefaultInput(), options, CancellationToken.None);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.Not.Null);
    }

    [Test]
    public void Should_Use_Custom_ErrorMessageOnFailure()
    {
        var options = DefaultOptions();
        options.ErrorMessageOnFailure = CustomErrorMessage;

        var exception = Assert.ThrowsAsync<Exception>(() =>
            LinkedIn.Request(DefaultInput(), options, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo(CustomErrorMessage));
    }

    private static Input DefaultInput() => new()
    {
        Method = Method.GET,
        Url = "http://[",
        Headers = Array.Empty<Header>(),
    };

    private static Options DefaultOptions() => new()
    {
        LinkedInAPIVersion = "202309",
        ConnectionTimeoutSeconds = 30,
        FollowRedirects = true,
        AutomaticCookieHandling = true,
    };
}
