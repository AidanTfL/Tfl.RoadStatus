namespace TfL.RoadStatus.TestHelpers.Mocks
{
    public class MockData
    {
        public static class TflApi
        {
            public static string RoadEndpoint = "*/Road/*";

            public class Ok
            {
                public static string RoadStatusForA3Response =
                    "[\r\n{\r\n\"$type\": \"Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities\",\r\n\"id\": \"a3\",\r\n\"displayName\": \"A3\",\r\n\"statusSeverity\": \"Good\",\r\n\"statusSeverityDescription\": \"No Exceptional Delays\",\r\n\"bounds\": \"[[-0.32751,51.32598],[-0.12639,51.48473]]\",\r\n\"envelope\": \"[[-0.32751,51.32598],[-0.32751,51.48473],[-0.12639,51.48473],[-0.12639,51.32598],[-0.32751,51.32598]]\",\r\n\"url\": \"/Road/a3\"\r\n}\r\n]";

                public static string RoadStatusForA1A2Response =
                    "[{\"$type\":\"Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities\",\"id\":\"a1\",\"displayName\":\"A1\",\"statusSeverity\":\"Good\",\"statusSeverityDescription\":\"No Exceptional Delays\",\"bounds\":\"[[-0.25616,51.5319],[-0.10234,51.6562]]\",\"envelope\":\"[[-0.25616,51.5319],[-0.25616,51.6562],[-0.10234,51.6562],[-0.10234,51.5319],[-0.25616,51.5319]]\",\"url\":\"/Road/a1\"},{\"$type\":\"Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities\",\"id\":\"a2\",\"displayName\":\"A2\",\"statusSeverity\":\"Good\",\"statusSeverityDescription\":\"No Exceptional Delays\",\"bounds\":\"[[-0.0857,51.44091],[0.17118,51.49438]]\",\"envelope\":\"[[-0.0857,51.44091],[-0.0857,51.49438],[0.17118,51.49438],[0.17118,51.44091],[-0.0857,51.44091]]\",\"url\":\"/Road/a2\"}]";

                public static string RoadStatusForA1A2CityRouteResponse =
                    "[{\"$type\":\"Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities\",\"id\":\"a1\",\"displayName\":\"A1\",\"statusSeverity\":\"Good\",\"statusSeverityDescription\":\"No Exceptional Delays\",\"bounds\":\"[[-0.25616,51.5319],[-0.10234,51.6562]]\",\"envelope\":\"[[-0.25616,51.5319],[-0.25616,51.6562],[-0.10234,51.6562],[-0.10234,51.5319],[-0.25616,51.5319]]\",\"url\":\"/Road/a1\"},{\"$type\":\"Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities\",\"id\":\"a2\",\"displayName\":\"A2\",\"statusSeverity\":\"Good\",\"statusSeverityDescription\":\"No Exceptional Delays\",\"bounds\":\"[[-0.0857,51.44091],[0.17118,51.49438]]\",\"envelope\":\"[[-0.0857,51.44091],[-0.0857,51.49438],[0.17118,51.49438],[0.17118,51.44091],[-0.0857,51.44091]]\",\"url\":\"/Road/a2\"},{\"$type\":\"Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities\",\"id\":\"city route\",\"displayName\":\"City Route\",\"group\":\"Central London Red Routes\",\"statusSeverity\":\"Closure\",\"statusSeverityDescription\":\"Closure\",\"bounds\":\"[[-0.2211,51.47224],[-0.02438,51.5319]]\",\"envelope\":\"[[-0.2211,51.47224],[-0.2211,51.5319],[-0.02438,51.5319],[-0.02438,51.47224],[-0.2211,51.47224]]\",\"url\":\"/Road/city+route\"}]";
            }

            public class NotFound
            {
                public static string RoadStatusFor1NonExistent =
                    "{\"$type\":\"Tfl.Api.Presentation.Entities.ApiError, Tfl.Api.Presentation.Entities\",\"timestampUtc\":\"2021-08-14T15:53:40.2835218Z\",\"exceptionType\":\"EntityNotFoundException\",\"httpStatusCode\":400,\"httpStatus\":\"BadRequest\",\"relativeUri\":\"/Road/RoadWithoutTraffic\",\"message\":\"The following road id is not recognised: RoadWithoutTraffic\"}";
            }

            public class BadRequest
            {
                public static string RoadStatusRequestUnrecognised =
                    "{\r\n    \"$type\": \"Tfl.Api.Presentation.Entities.ApiError, Tfl.Api.Presentation.Entities\",\r\n    \"timestampUtc\": \"2021-08-14T17:00:06.2287865Z\",\r\n    \"exceptionType\": \"EntityNotFoundException\",\r\n    \"httpStatusCode\": 400,\r\n    \"httpStatus\": \"BadRequest\",\r\n    \"relativeUri\": \"/Road/RoadWithoutTraffic/Status\",\r\n    \"message\": \"The following road id is not recognised: RoadWithoutTraffic\"\r\n}";
            }

            public class TooManyRequests
            {
                public static string InvalidApiKey = "Invalid app_key is provided.";
            }
        }
    }
}