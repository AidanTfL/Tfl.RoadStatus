using System;

namespace TfL.RoadStatus.Application
{
    public class AppConfig
    {
        public string[] RoadIds { get; set; }
        public Uri ApiUrl { get; set; }
        public string AppId { get; set; }
        public string ApiKey { get; set; }
        public Logging Logging { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string MicrosoftHostingLifetime { get; set; }
    }
}