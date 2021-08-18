using System;
using System.Net;
using TfL.RoadStatus.Domain;

namespace TfL.RoadStatus.Application.Exceptions
{
    public class ApiClientException : Exception
    {
        public ApiClientException(HttpStatusCode? statusCode = null, Exception innerException = null,
            ApiError error = null, string customMessage = null)
            : base($"{ExceptionMsgBuilder.Build<ApiClientException>(statusCode, error, customMessage)}", innerException)
        {
        }

        internal class ExceptionMsgBuilder
        {
            public static string Build<T>(HttpStatusCode? statusCode = null, ApiError error = null,
                string customMessage = null) where T : ApiClientException
            {
                if (statusCode == null) return customMessage;

                switch (statusCode)
                {
                    case HttpStatusCode.TooManyRequests:
                        return customMessage;

                    case HttpStatusCode.ServiceUnavailable:
                    case HttpStatusCode.InternalServerError:
                        return 
                            $"{typeof(T).Name}: The Tfl Api is unavailable. Please try again in a few minutes. Status Code: {statusCode}";

                    default: //Bad Request
                        return
                            $"An api error ({(int) statusCode}: {statusCode}) was received. If this error persists, please contact the developer." +
                            $"\nAn update to this application may be required. Detailed Error: {error?.Message}";
                }
            }
        }
    }
}