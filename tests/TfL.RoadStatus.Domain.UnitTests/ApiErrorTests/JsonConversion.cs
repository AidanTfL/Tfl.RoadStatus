using Newtonsoft.Json;
using NUnit.Framework;
using TfL.RoadStatus.TestHelpers.Mocks;

namespace TfL.RoadStatus.Domain.UnitTests.ApiErrorTests
{
    [TestOf(typeof(ApiError))]
    public class JsonConversionTests
    {
        [Test]
        public void GivenValidJsonRepresentation_WhenDeserialize_ThenCreatesObject()
        {
            var apiErrorAsJson = MockData.TflApi.BadRequest.RoadStatusRequestUnrecognised;
            var apiError = default(ApiError);

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() => { apiError = JsonConvert.DeserializeObject<ApiError>(apiErrorAsJson); });

                Assert.IsInstanceOf<ApiError>(apiError);

                foreach (var prop in typeof(ApiError).GetProperties())
                    Assert.IsNotNull(prop.GetValue(apiError), $"{prop.Name} should not be null");
            });
        }
    }
}