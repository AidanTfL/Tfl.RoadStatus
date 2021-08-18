using System;
using System.Net;

namespace TfL.RoadStatus.Domain
{
    public class ApiError
    {
        public DateTimeOffset TimestampUtc { get; set; }
        public string ExceptionType { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string HttpStatus { get; set; }
        public string RelativeUri { get; set; }
        public string Message { get; set; }
    }
}