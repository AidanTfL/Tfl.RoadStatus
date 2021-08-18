using System;
using TfL.RoadStatus.Application.GetRoadStatus;

namespace TfL.RoadStatus.Application.Interfaces
{
    public interface IUrlBuilder
    {
        Uri BuildFrom(GetRoadStatusQuery request);
    }
}