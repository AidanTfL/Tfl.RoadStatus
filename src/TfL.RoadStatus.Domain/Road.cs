namespace TfL.RoadStatus.Domain
{
    //todo: consider refactoring to use the NSwag auto-generated RoadCorridor entity instead

    public class Road
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string StatusSeverity { get; set; }

        public string StatusSeverityDescription { get; set; }

        public string Bounds { get; set; }

        public string Envelope { get; set; }

        public string Url { get; set; }
    }
}