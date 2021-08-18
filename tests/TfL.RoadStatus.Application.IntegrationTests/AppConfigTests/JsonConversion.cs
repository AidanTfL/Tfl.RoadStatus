using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TfL.RoadStatus.Application.IntegrationTests.AppConfigTests
{
    [TestOf(typeof(AppConfig))]
    public class JsonConversionTests
    {
        [Test]
        public void GivenValidAppSettings_WhenDeserialize_ThenCreatesObject()
        {
            var appConfigAsJson = File.ReadAllText(Testing.MockAppSettingsFileName);

            var appConfig = default(AppConfig);

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() => { appConfig = JsonConvert.DeserializeObject<AppConfig>(appConfigAsJson); });
                Assert.IsInstanceOf<AppConfig>(appConfig);
            });
        }

        [Test]
        public void GivenValidAppSettings_WhenDeserialize_UsingConfigBuilder_ThenCreatesObject()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(Testing.MockAppSettingsFileName);

            var appConfig = default(AppConfig);

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() =>
                {
                    var configurationRoot = builder.Build();
                    appConfig = configurationRoot.Get<AppConfig>();
                });
                Assert.IsInstanceOf<AppConfig>(appConfig);
            });
        }
    }
}