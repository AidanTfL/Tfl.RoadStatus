using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;
using TfL.RoadStatus.Application.GetRoadStatus;
using static TfL.RoadStatus.Application.IntegrationTests.Testing;
using static TfL.RoadStatus.TestHelpers.TestCases.GetRoadStatusQueryTestConstants;

namespace TfL.RoadStatus.Application.IntegrationTests.GetRoadStatusTests
{
    [TestOf(typeof(GetRoadStatusQuery))]
    public class GetRoadStatusTests
    {
        [Test]
        [TestCaseSource(typeof(HappyPath.Get), nameof(HappyPath.Get.All), Category = nameof(HappyPath))]
        public async Task GivenGetRoadStatusQuery_WhenValid_ThenReturnsIListRoad(GetRoadStatusQuery query)
        {
            var roadStatusList = await SendAsync(query);
            roadStatusList.Should().NotBeNullOrEmpty();
        }

        [Test]
        [TestCaseSource(typeof(UnhappyPath.Get), nameof(HappyPath.Get.All), Category = nameof(HappyPath))]
        public void GivenGetRoadStatusQuery_WhenInvalid_ThenThrowsException(GetRoadStatusQuery query)
        {
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                var roadStatusList = await SendAsync(query);
                roadStatusList.Should().NotBeNullOrEmpty();
            });
        }

        [Test]
        public void GivenGetRoadStatusQuery_WhenEmpty_ThenThrowsFluentValidationException()
        {
            var query = new GetRoadStatusQuery();

            Assert.ThrowsAsync<ValidationException>(async () => { await SendAsync(query); });
        }
    }
}