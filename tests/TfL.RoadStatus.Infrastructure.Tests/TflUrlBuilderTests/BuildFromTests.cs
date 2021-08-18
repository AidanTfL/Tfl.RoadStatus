using System;
using Flurl;
using NUnit.Framework;
using TfL.RoadStatus.Application.GetRoadStatus;
using static TfL.RoadStatus.TestHelpers.TestCases.GetRoadStatusQueryTestConstants;

namespace TfL.RoadStatus.Infrastructure.Tests.TflUrlBuilderTests
{
    [TestOf(typeof(TflUrlBuilder))]
    public class BuildFromTests
    {
        [Test]
        [TestCaseSource(typeof(HappyPath.Get), nameof(HappyPath.Get.All), Category = nameof(HappyPath))]
        public void GivenGetRoadStatusQuery_WhenValid_ThenReturnUrl(GetRoadStatusQuery request)
        {
            var expectedUrl = new Url($"{request.ApiUri}Road/{string.Join(",", request.RoadIds)}");

            if (!string.IsNullOrWhiteSpace(request.AppId)) expectedUrl.SetQueryParam("app_id", request.AppId);
            if (!string.IsNullOrWhiteSpace(request.ApiKey)) expectedUrl.SetQueryParam("app_key", request.ApiKey);

            var actualUrl = new TflUrlBuilder().BuildFrom(request);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedUrl.ToUri(), actualUrl);

                Assert.AreEqual(request.ApiUri.Host.ToLower(), actualUrl.Host.ToLower(), "Url hostnames do not match");

                Assert.AreEqual(expectedUrl.ToUri().AbsolutePath, actualUrl.AbsolutePath, "Url paths do not match");

                CollectionAssert.AreEqual(expectedUrl.Query, actualUrl.Query.TrimStart('?'),
                    "Url query strings do not not match");
            });
        }

        [Test]
        [TestCaseSource(typeof(HappyPath.Get), nameof(HappyPath.Get.GenerateValidWithMultipleRoadIds), Category = nameof(HappyPath))]
        public void GivenGetRoadStatusQuery_WhenValid_AndMultipleRoadIds_ThenRoadIdsAreCsv(GetRoadStatusQuery request)
        {
            var expectedRoadIdsComponent = string.Join(",", request.RoadIds).Replace(" ", "%20");

            var actualUrl = new TflUrlBuilder().BuildFrom(request);

            var actualRoadIdsComponent = actualUrl.AbsolutePath.Replace("/Road/", string.Empty);

            Assert.AreEqual(expectedRoadIdsComponent, actualRoadIdsComponent);
        }

        [Test]
        [TestCaseSource(typeof(HappyPath.Get), nameof(HappyPath.Get.GenerateValidWithSpaces), Category = nameof(HappyPath))]
        public void GivenGetRoadStatusQuery_WhenValid_AndRoadIdContainsSpace_ThenSpaceIsEscaped(GetRoadStatusQuery request)
        {
            var expectedRoadIdsComponent = string.Join(",", request.RoadIds).Replace(" ", "%20");

            var actualUrl = new TflUrlBuilder().BuildFrom(request);

            var actualRoadIdsComponent = actualUrl.AbsolutePath.Replace("/Road/", string.Empty);

            Assert.AreEqual(expectedRoadIdsComponent, actualRoadIdsComponent);
        }

        [Test]
        [TestCaseSource(typeof(UnhappyPath.Get), nameof(UnhappyPath.Get.All), Category = nameof(UnhappyPath))]
        public void GivenGetRoadStatusQuery_WhenInvalid_WithNullOrEmptyProps_ThenThrowAnArgumentNullException(GetRoadStatusQuery request)
        {
            Assert.Multiple(() =>
            {
                var exception = Assert.Throws<ArgumentNullException>(() =>
                {
                    var actualUrl = new TflUrlBuilder().BuildFrom(request);
                    Assert.IsNotInstanceOf<Url>(actualUrl);
                });
                Assert.IsNotNull(exception.Message);
            });
        }
    }
}