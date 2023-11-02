namespace Frends.LinkedIn.SearchAdAnalytics.Tests
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Frends.LinkedIn.SearchAdAnalytics.Definitions;
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
                Finder = Finder.Analytics,
                Pivot = Pivot.NONE,
                StartDate = DateTime.Now.AddDays(-10).ToString(),
                Campaigns = new string[] { "urn:li:sponsoredCampaign:99966515" },
                Creatives = Array.Empty<string>(),
                Metrics = Array.Empty<string>(),
                TimeGranularity = TimeGranularity.ALL,
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
        public async Task SearchAdAnalytics_BasicTest()
        {
            var result = await LinkedIn.SearchAdAnalytics(_filter, _options, default);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task SearchAdAnalytics_WithMetrics()
        {
            _filter.Metrics = new string[] { "clicks", "comments" };
            var result = await LinkedIn.SearchAdAnalytics(_filter, _options, default);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task SearchAdAnalytics_WithOver20Metrics()
        {
            _filter.Metrics = new string[]
            {
                "clicks",
                "comments",
                "cardClicks",
                "cardImpressions",
                "commentLikes",
                "companyPageClicks",
                "conversionValueInLocalCurrency",
                "costInLocalCurrency",
                "costInUsd",
                "dateRange",
                "documentCompletions",
                "follows",
                "fullScreenPlays",
                "headlineClicks",
                "impressions",
                "jobApplications",
                "jobApplyClicks",
                "landingPageClicks",
                "leadGenerationMailContactInfoShares",
                "leadGenerationMailInterestedClicks",
                "likes",
                "opens",
            };
            var ex = Assert.ThrowsAsync<WebException>(async () => await LinkedIn.SearchAdAnalytics(_filter, _options, default));
            Console.WriteLine(ex.Message);
            Assert.IsTrue(ex.Message.Contains(@"""status"": 400"));
            Assert.IsTrue(ex.Message.Contains(@"""message"": ""Too many fields requested. Maximum possible fields to request: 20"""));

            _options.ThrowExceptionOnErrorResponse = false;
            var result = await LinkedIn.SearchAdAnalytics(_filter, _options, default);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task SearchAdAnalytics_InvalidAccessToken_ThrowsException()
        {
            _options.Token = "invalid_access_token"; // Simulate an invalid token
            var ex = Assert.ThrowsAsync<WebException>(async () => await LinkedIn.SearchAdAnalytics(_filter, _options, default));
            Assert.IsTrue(ex.Message.Contains(@"""status"": 401"));

            _options.ThrowExceptionOnErrorResponse = false;
            var result = await LinkedIn.SearchAdAnalytics(_filter, _options, default);
            Assert.AreEqual(401, result.StatusCode);
        }
    }
}