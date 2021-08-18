using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TfL.RoadStatus.ConsoleUI.Filters;
using static System.Console;

namespace TfL.RoadStatus.ConsoleUI
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var argsParser = new Parser(with => with.CaseSensitive = false);
            var parserResult = argsParser.ParseArguments<ConsoleArgs>(args);

            await parserResult.WithParsedAsync(RunAsync);

            parserResult.WithNotParsed(x =>
            {
                var helpText = HelpText.AutoBuild(parserResult, h =>
                {
                    h.AutoHelp = false;
                    h.AutoVersion = false;
                    h.Copyright = string.Empty;
                    return HelpText.DefaultParsingErrorsHandler(parserResult, h);
                }, e => e);

                WriteLine(helpText);
                ConsoleExceptionFilter.HandleArgsParserFailure(helpText);
            });
        }

        public static async Task RunAsync(ConsoleArgs args)
        {
            using var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    new Startup(hostContext.Configuration)
                        .Configure(configBuilder, hostContext.HostingEnvironment, args);
                })
                .ConfigureServices((hostContext, serviceCollection) =>
                    new Startup(hostContext.Configuration)
                        .ConfigureServices(serviceCollection))
                .ConfigureLogging(builder => builder.AddConsole())
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }
}