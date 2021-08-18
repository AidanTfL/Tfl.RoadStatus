using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CommandLine;

namespace TfL.RoadStatus.ConsoleUI
{
    public class ConsoleArgs
    {
        [Value(0, HelpText = "Space-delimited. Note: RoadIds with spaces must be escaped e.g: city%20route",
            MetaName = nameof(RoadIds), Required = true)]
        public IEnumerable<string> RoadIds { get; set; }

        [Option(HelpText = "Required, but can be set as an environment variable instead. E.g: https://api.tfl.gov.uk.")]
        public string ApiUrl { get; set; }

        [Option(HelpText = "Optional. (Included only to support legacy registrations)")]
        public string AppId { get; set; }

        [Option(HelpText = "Recommended. Register at https://api-portal.tfl.gov.uk")]
        public string ApiKey { get; set; }
    }

    public static class ConsoleArgsExtensions
    {
        public static MemoryStream ToJsonStream(this ConsoleArgs args)
        {
            var argsAsJson = 
                JsonSerializer.SerializeToUtf8Bytes(args, new JsonSerializerOptions {IgnoreNullValues = true});

            return new MemoryStream(argsAsJson);
        }
    }
}