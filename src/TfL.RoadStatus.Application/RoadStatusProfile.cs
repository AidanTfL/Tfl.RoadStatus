using AutoMapper;
using TfL.RoadStatus.Application.GetRoadStatus;
using Tfl.RoadStatus.Domain;
using TfL.RoadStatus.Domain;

namespace TfL.RoadStatus.Application
{
    public class RoadStatusProfile : Profile
    {
        public RoadStatusProfile()
        {
            CreateMap<Road, GetRoadStatusResponse>()
                .ForMember(r => r.RawStatusSeverity, m =>
                    m.MapFrom(r => r.StatusSeverity));

            CreateMap<RoadCorridor, GetRoadStatusResponse>()
                .ForMember(r => r.RawStatusSeverity, m =>
                    m.MapFrom(r => r.StatusSeverity));
        }
    }
}