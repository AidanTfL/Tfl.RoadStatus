using System;
using Flurl;
using TfL.RoadStatus.Application.GetRoadStatus;
using TfL.RoadStatus.Application.Interfaces;

namespace TfL.RoadStatus.Infrastructure
{
    public class TflUrlBuilder : IUrlBuilder
    {
        public Uri BuildFrom(GetRoadStatusQuery request)
        {
            Validate(request);
            var roadIdsCsv = string.Join(',', request.RoadIds);

            var url = new Url(request.ApiUri) {Path = $"Road/{roadIdsCsv}"};

            if (!string.IsNullOrWhiteSpace(request.AppId))
                url.SetQueryParam("app_id", request.AppId);

            if (!string.IsNullOrWhiteSpace(request.ApiKey)) 
                url.SetQueryParam("app_key", request.ApiKey);

            return url.ToUri();
        }

        private static void Validate(GetRoadStatusQuery request)
        {
            if (request.ApiUri == null)
                throw new ArgumentNullException(nameof(GetRoadStatusQuery.RoadIds));

            //AppId and ApiKey are optional

            if (request.RoadIds == null || request.RoadIds.Length == 0)
                throw new ArgumentNullException(nameof(GetRoadStatusQuery.RoadIds));
        }
    }
}