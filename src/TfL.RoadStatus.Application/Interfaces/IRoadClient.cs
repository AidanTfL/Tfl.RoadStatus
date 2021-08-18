using System.Collections.Generic;
using System.Threading.Tasks;
using TfL.RoadStatus.Application.GetRoadStatus;
using TfL.RoadStatus.Domain;

namespace TfL.RoadStatus.Application.Interfaces
{
    //todo: Remove this, if TflRoadClient is refactored to implement the NSwag auto-generated interface

    public interface IRoadClient
    {
        Task<IList<Road>> GetRoadStatusAsync(GetRoadStatusQuery request);
    }
}