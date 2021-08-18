using System;
using System.Collections.Generic;
using MediatR;

namespace TfL.RoadStatus.Application.GetRoadStatus
{
    public class GetRoadStatusQuery : IRequest<IList<GetRoadStatusResponse>>
    {
        public string[] RoadIds { get; set; }
        public Uri ApiUri { get; set; }
        public string AppId { get; set; }
        public string ApiKey { get; set; }
    }
}