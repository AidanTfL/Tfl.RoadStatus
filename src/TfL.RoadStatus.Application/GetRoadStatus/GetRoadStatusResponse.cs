using System;
using TfL.RoadStatus.Domain;

namespace TfL.RoadStatus.Application.GetRoadStatus
{
    public class GetRoadStatusResponse
    {
        public string DisplayName { get; set; }
        public string RawStatusSeverity { get; set; }
        public string StatusSeverityDescription { get; set; }

        [Obsolete("Avoid using this, until confirmed these are the correct severity levels.", true)]
        public SeverityLevel? StatusSeverity
        {
            get
            {
                Enum.TryParse<SeverityLevel>(RawStatusSeverity, out var result);
                return result;
            }
        }
    }
}