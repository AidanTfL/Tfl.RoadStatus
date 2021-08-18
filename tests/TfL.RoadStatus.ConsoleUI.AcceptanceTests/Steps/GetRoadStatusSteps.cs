using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TfL.RoadStatus.Application;

namespace TfL.RoadStatus.ConsoleUI.AcceptanceTests.Steps
{
    [Binding]
    public class GetRoadStatusSteps
    {
        private readonly string _consoleAppExePath;
        private readonly ConsoleArgs _consoleArgs;

        private Process _consoleUiProcess;

        public GetRoadStatusSteps(AppConfig appConfig)
        {
            _consoleAppExePath = GetConsoleAppExePath();

            _consoleArgs = new ConsoleArgs
            {
                ApiUrl = appConfig.ApiUrl.ToString(),
                AppId = appConfig.AppId,
                ApiKey = appConfig.ApiKey,
                RoadIds = appConfig.RoadIds
            };
        }

        public string GetConsoleAppExePath()
        {
            var executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var directory = new DirectoryInfo(executingAssemblyPath);

            while (directory?.Name != "tests") directory = directory?.Parent;
            var rootDirectory = directory?.Parent;

            var applicationName = typeof(Startup).Namespace;
            var applicationExeDirectory = Path.Combine(rootDirectory?.FullName, $"src\\{applicationName}");
            var applicationExePath = Path.Combine(applicationExeDirectory, "bin\\debug\\netcoreapp3.1\\", $"{applicationName}.exe");

            return applicationExePath;
        }

        [Given("a valid roadID (.*) is specified")]
        public void GivenAValid_RoadId_IsSpecified(string roadId)
        {
            _consoleArgs.RoadIds = new[] {roadId};
        }

        [Given("an invalid roadID (.*) is specified")]
        public void GivenAnInvalid_RoadId_IsSpecified(string roadId)
        {
            _consoleArgs.RoadIds = new[] {roadId};
        }

        [When]
        public void WhenTheClientIsRun()
        {
            var argsBuilder = new StringBuilder();

            argsBuilder.Append(string.Join(' ', _consoleArgs.RoadIds));

            if (!string.IsNullOrWhiteSpace(_consoleArgs.ApiUrl)) argsBuilder.Append(" --apiurl ").Append(_consoleArgs.ApiUrl);
            if (!string.IsNullOrWhiteSpace(_consoleArgs.AppId)) argsBuilder.Append(" --appid ").Append(_consoleArgs.AppId);
            if (!string.IsNullOrWhiteSpace(_consoleArgs.ApiKey)) argsBuilder.Append(" --apikey ").Append(_consoleArgs.ApiKey);

            var arguments = argsBuilder.ToString().Trim();

            var processStartInfo = new ProcessStartInfo
            {
                FileName = _consoleAppExePath,
                Arguments = arguments,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            _consoleUiProcess = Process.Start(processStartInfo);
            _consoleUiProcess?.WaitForExit(TimeSpan.FromSeconds(15).Milliseconds); //timeout after 15 seconds
        }

        [Then("the road displayName (.*) should be displayed")]
        public void ThenTheRoadDisplayNameShouldBeDisplayed(string expectedDisplayName)
        {
            var output = _consoleUiProcess.StandardOutput.ReadToEnd().Split('\n');
            output[0]?.Should().Be($"The status of the {expectedDisplayName} is as follows\r");
        }

        [Then("the road statusSeverity (.*) should be displayed")]
        public void ThenTheRoadStatusSeverityShouldBeDisplayed(string expectedStatusSeverity)
        {
            Assert.Multiple(() =>
            {
                var output = _consoleUiProcess.StandardOutput.ReadToEnd().Split('\n');

                var secondLine = output[1]?.TrimEnd();

                var regex = new Regex(@"\t Road Status is (?!\s*$).+"); //not empty
                expectedStatusSeverity.Should().Be("regex not empty");
                secondLine.Should().MatchRegex(regex.ToString());
            });
        }

        [Then("the road statusSeverityDescription (.*) should be displayed")]
        public void ThenTheRoadStatusSeverityDescriptionShouldBeDisplayed(string expectedStatusSeverityDescription)
        {
            Assert.Multiple((() =>
            {
                var output = _consoleUiProcess.StandardOutput.ReadToEnd().Split('\n');

                var thirdLine = output[2]?.TrimEnd();

                var regex = new Regex(@"\t Road Status Description is (?!\s*$).+"); //not empty
                expectedStatusSeverityDescription.Should().Be("regex not empty");
                thirdLine.Should().MatchRegex(regex.ToString());
            }));
        }

        [Then("the application should return an informative error (.*)")]
        public void ThenTheApplicationShouldReturnAnInformativeError(string expectedError)
        {
            Assert.Multiple(() =>
            {
                var firstLine = _consoleUiProcess.StandardOutput.ReadLine()?.TrimEnd();
                firstLine.Should().Be(expectedError);

                if (_consoleArgs.RoadIds.Any()) return;

                var expected = "TfL.RoadStatus.ConsoleUI 1.0.0\r\n\r\nERROR(S):\r\n  A required value not bound to option name is missing.\r\n\r\n  --apiurl            Required, but can be set as an environment variable\r\n                      instead. E.g: https://api.tfl.gov.uk.\r\n\r\n  --appid             Optional. (Included only to support legacy registrations)\r\n\r\n  --apikey            Required, but can be set as an environment variable\r\n                      instead. Register at https://api-portal.tfl.gov.uk\r\n\r\n  RoadIds (pos. 0)    Required. Space-delimited. Note: RoadIds with spaces must\r\n                      be escaped e.g: city%20route";
                var entireOutput = _consoleUiProcess.StandardOutput.ReadToEnd().TrimEnd();
                entireOutput.Should().Be(expected);
            });
        }

        [Then("the application should exit with a non-zero System Error code (.*)")]
        public void ThenTheApplicationShouldExitWithANonZeroSystemErrorCode(int errorCode)
        {
            Assert.Multiple((() =>
            {
                _consoleUiProcess.WaitForExit();
                _consoleUiProcess.ExitCode.Should().NotBe(0);
                _consoleUiProcess.ExitCode.Should().Be(errorCode);
            }));
        }
    }
}