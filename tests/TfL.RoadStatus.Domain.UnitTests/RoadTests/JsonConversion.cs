using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using TfL.RoadStatus.TestHelpers.Mocks;

namespace TfL.RoadStatus.Domain.UnitTests.RoadTests
{
    [TestOf(typeof(Road))]
    public class JsonConversionTests
    {
        [Test]
        public void GivenValidJsonListRepresentation_WhenDeserialize_ThenCreatesIListObject()
        {
            var roadAsJson = MockData.TflApi.Ok.RoadStatusForA3Response;
            var roadStatusList = default(IList<Road>);

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() => { roadStatusList = JsonConvert.DeserializeObject<List<Road>>(roadAsJson); });

                Assert.IsInstanceOf<IList<Road>>(roadStatusList);

                Parallel.ForEach(roadStatusList, road =>
                {
                    foreach (var prop in typeof(Road).GetProperties())
                        Assert.IsNotNull(prop.GetValue(road), $"{prop.Name} should not be null");
                });
            });
        }
    }
}