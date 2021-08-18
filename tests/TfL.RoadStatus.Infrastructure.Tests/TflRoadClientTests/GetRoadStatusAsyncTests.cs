using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Flurl.Http.Testing;
using Moq;
using NUnit.Framework;
using TfL.RoadStatus.Application.Exceptions;
using TfL.RoadStatus.Application.GetRoadStatus;
using TfL.RoadStatus.Domain;
using TfL.RoadStatus.TestHelpers.Mocks;
using static TfL.RoadStatus.TestHelpers.TestCases.GetRoadStatusQueryTestConstants;

namespace TfL.RoadStatus.Infrastructure.Tests.TflRoadClientTests
{
    [TestOf(typeof(TflRoadClient))]
    public class GetRoadStatusAsyncTests
    {
        private static string RoadEndpoint => MockData.TflApi.RoadEndpoint;

        [Test]
        [TestCaseSource(typeof(HappyPath.Get), nameof(HappyPath.Get.All), Category = nameof(HappyPath))]
        public async Task GivenGetRoadStatusQuery_WhenValid_ThenReturnIListRoad(GetRoadStatusQuery request)
        {
            var mockHttp = new HttpTest();
            mockHttp
                .ForCallsTo(RoadEndpoint)
                .WithVerb(HttpMethod.Get)
                .RespondWith(MockData.TflApi.Ok.RoadStatusForA3Response);

            var client = GenerateTflRoadClient();
            var results = await client.GetRoadStatusAsync(request);

            Assert.Multiple(() =>
            {
                CollectionAssert.IsNotEmpty(results);
                CollectionAssert.AllItemsAreInstancesOfType(results, typeof(Road));
                CollectionAssert.AllItemsAreUnique(results);
                mockHttp.ShouldHaveCalled(RoadEndpoint);
            });
        }

        [Test]
        [TestCaseSource(typeof(HappyPath.EdgeCase), nameof(HappyPath.EdgeCase.GetValidWithSingleRoadId),
            Category = nameof(HappyPath))]
        public void GivenARequest_WhenTimeoutOccurs_ThenThrowsApiClientException_CheckInternetConnection(
            GetRoadStatusQuery request)
        {
            var expectedExceptionMsg = "Timeout: Please check your internet connection";

            var mockHttp = new HttpTest();
            mockHttp.SimulateTimeout();

            var client = GenerateTflRoadClient();

            Assert.Multiple(() =>
            {
                var exception = Assert.ThrowsAsync<ApiClientException>(async () =>
                {
                    var response = await client.GetRoadStatusAsync(request);
                    Warn.If(response != null);
                });

                Assert.AreEqual(expectedExceptionMsg, exception.Message);
            });
        }

        [Test]
        [TestCaseSource(typeof(HappyPath.EdgeCase), nameof(HappyPath.EdgeCase.GetValidWithSingleRoadId), Category = nameof(HappyPath))]
        public void GivenARequest_WhenDNSErrorOccurs_ThenThrowsApiClientException_CheckInternetConnection(
            GetRoadStatusQuery request)
        {
            var expectedExceptionMsg = "No such host is known.";

            new HttpTest().Dispose(); // disposing mock, results in all subsequent requests being refused
            var client = GenerateTflRoadClient();

            Assert.Multiple(() =>
            {
                var exception = Assert.ThrowsAsync<ApiClientException>(async () =>
                {
                    var response = await client.GetRoadStatusAsync(request);
                    Warn.If(response != null);
                });

                Assert.AreEqual(expectedExceptionMsg, exception.Message);
            });
        }


        [Test]
        [TestCaseSource(typeof(UnhappyPath.EdgeCase), nameof(UnhappyPath.EdgeCase.GetNotFoundRoadId), Category = nameof(UnhappyPath))]
        public void GivenARequest_WhenAnyRoadIdNotFound_ThenThrowsNotFoundException(GetRoadStatusQuery request)
        {
            var expectedExceptionMsg = $"{request.RoadIds.First()} is not a valid road";

            var mockHttp = new HttpTest();
            mockHttp
                .ForCallsTo(RoadEndpoint)
                .WithVerb(HttpMethod.Get)
                .RespondWith(MockData.TflApi.NotFound.RoadStatusFor1NonExistent, 404);

            var client = GenerateTflRoadClient();

            var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                var response = await client.GetRoadStatusAsync(request);
                Warn.If(response != null);
            });

            Assert.AreEqual(expectedExceptionMsg, exception.Message);
        }

        [Test]
        [TestCaseSource(typeof(UnhappyPath.EdgeCase), nameof(UnhappyPath.EdgeCase.GetNotFoundRoadId), Category = nameof(UnhappyPath))]
        public void GivenARequest_WhenApiReturnsBadRequest_ThenThrowsApiClientException_ApiLikelyToHaveChanged(
            GetRoadStatusQuery request)
        {
            var expectedExceptionMsg =
                "An api error (400: BadRequest) was received. If this error persists, please contact the developer." +
                "\nAn update to this application may be required. Detailed Error: The following road id is not recognised: RoadWithoutTraffic";

            var mockHttp = new HttpTest();
            mockHttp
                .ForCallsTo(RoadEndpoint)
                .WithVerb(HttpMethod.Get)
                .RespondWith(MockData.TflApi.BadRequest.RoadStatusRequestUnrecognised, 400);

            var client = GenerateTflRoadClient();

            Assert.Multiple(() =>
            {
                var exception = Assert.ThrowsAsync<ApiClientException>(async () =>
                {
                    var response = await client.GetRoadStatusAsync(request);
                    Warn.If(response != null);
                });

                Assert.AreEqual(expectedExceptionMsg, exception.Message);
            });
        }

        [Test]
        [TestCaseSource(typeof(UnhappyPath.EdgeCase), nameof(UnhappyPath.EdgeCase.GetNotFoundRoadId), Category = nameof(UnhappyPath))]
        public void GivenARequest_WhenApiReturnsTooManyRequests_ThenThrowsApiClientException_InvalidApiKey(
            GetRoadStatusQuery request)
        {
            var expectedExceptionMsg = "Invalid app_key is provided.";

            var mockHttp = new HttpTest();
            mockHttp
                .ForCallsTo(RoadEndpoint)
                .WithVerb(HttpMethod.Get)
                .RespondWith(MockData.TflApi.TooManyRequests.InvalidApiKey, 429);

            var client = GenerateTflRoadClient();

            Assert.Multiple(() =>
            {
                var exception = Assert.ThrowsAsync<ApiClientException>(async () =>
                {
                    var response = await client.GetRoadStatusAsync(request);
                    Warn.If(response != null);
                });

                Assert.AreEqual(expectedExceptionMsg, exception.Message);
            });
        }


        [Test]
        [TestCaseSource(typeof(UnhappyPath.EdgeCase), nameof(UnhappyPath.EdgeCase.GetNotFoundRoadId), Category = nameof(UnhappyPath))]
        public void GivenARequest_WhenApiReturnsServiceUnavailable_ThenThrowsApiClientException_TryAgain(
            GetRoadStatusQuery request)
        {
            var expectedExceptionMsg =
                $"{nameof(ApiClientException)}: The Tfl Api is unavailable. Please try again in a few minutes. Status Code: {HttpStatusCode.ServiceUnavailable}";

            var mockHttp = new HttpTest();
            mockHttp
                .ForCallsTo(RoadEndpoint)
                .WithVerb(HttpMethod.Get)
                .RespondWith(string.Empty, 503);

            var client = GenerateTflRoadClient();

            Assert.Multiple(() =>
            {
                var exception = Assert.ThrowsAsync<ApiClientException>(async () =>
                {
                    var response = await client.GetRoadStatusAsync(request);
                    Warn.If(response != null);
                });

                Assert.AreEqual(expectedExceptionMsg, exception?.Message);
            });
        }


        private static TflRoadClient GenerateTflRoadClient()
        {
            var urlBuilder = new TflUrlBuilder();

            var mockClientFactory = new Mock<IFlurlClientFactory>();
            mockClientFactory.Setup(x => x.Get(It.IsAny<Url>())).Returns(new FlurlClient());

            return new TflRoadClient(urlBuilder, mockClientFactory.Object);
        }
    }
}