using System.Threading.Tasks;
using Flurl.Http.Testing;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;
using TfL.RoadStatus.ConsoleUI;
using TfL.RoadStatus.TestHelpers.Mocks;

namespace TfL.RoadStatus.Application.IntegrationTests
{
    [SetUpFixture]
    public class Testing
    {
        public static readonly string MockAppSettingsFileName = "appsettings.integrationtests.json";

        private static IConfigurationRoot _configuration;
        private static IServiceScopeFactory _scopeFactory;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile(MockAppSettingsFileName);

            _configuration = builder.Build();

            var startup = new Startup(_configuration);

            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IHostEnvironment>(w =>
                w.EnvironmentName == "Production" &&
                w.ApplicationName == nameof(RoadStatus)));

            services.AddLogging();

            startup.ConfigureServices(services);

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            MockAllHttpRequests();
        }

        public void MockAllHttpRequests()
        {
            var mockHttpConfig = new HttpTest();
            mockHttpConfig
                .ForCallsTo(MockData.TflApi.RoadEndpoint)
                .RespondWith(MockData.TflApi.Ok.RoadStatusForA1A2CityRouteResponse);
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<ISender>();

            return await mediator.Send(request);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}