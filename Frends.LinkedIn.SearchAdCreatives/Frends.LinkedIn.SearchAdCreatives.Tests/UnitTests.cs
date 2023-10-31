namespace Frends.LinkedIn.SearchAdCreatives.Tests
{
    using System;
    using System.Threading.Tasks;
    using Frends.LinkedIn.SearchAdCreatives.Definitions;
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
                AdAccountId = "512500029",
                AdCampaignUrns = Array.Empty<AdCampaign>(),
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
        public async Task SearchCampaigns_TestGetAdCreative()
        {
            _filter.GetSingleCreative = true;
            _filter.AdCreativeUrn = "urn:li:sponsoredCreative:119962155";
            var result = await LinkedIn.SearchAdCreatives(_filter, _options, default);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public async Task SearchCampaigns_TestGetWithCampaignUrns()
        {
            _filter.AdCampaignUrns = new AdCampaign[] { new AdCampaign { AdCampaignUrn = "urn:li:sponsoredCampaign:99966515" } };
            var result = await LinkedIn.SearchAdCreatives(_filter, _options, default);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task SearchCampaigns_TestGetWithCreativeUrns()
        {
            _filter.AdCreativesUrns = new AdCreative[] { new AdCreative { AdCreativeUrn = "urn:li:sponsoredCreative:119962155" } };
            var result = await LinkedIn.SearchAdCreatives(_filter, _options, default);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}