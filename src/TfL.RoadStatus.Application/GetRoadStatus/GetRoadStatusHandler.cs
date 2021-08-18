using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TfL.RoadStatus.Application.Interfaces;

namespace TfL.RoadStatus.Application.GetRoadStatus
{
    public class GetRoadStatusHandler : IRequestHandler<GetRoadStatusQuery, IList<GetRoadStatusResponse>>
    {
        private readonly IRoadClient _client;
        private readonly IMapper _mapper;
        private readonly IValidator<GetRoadStatusQuery> _validator;

        public GetRoadStatusHandler(IValidator<GetRoadStatusQuery> validator, IRoadClient client, IMapper mapper)
        {
            _validator = validator;
            _client = client;
            _mapper = mapper;
        }

        public async Task<IList<GetRoadStatusResponse>> Handle(GetRoadStatusQuery request,
            CancellationToken cancellationToken)
        {
            var validatorResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid) throw new ValidationException(validatorResult.Errors);

            var roadStatusList = _client.GetRoadStatusAsync(request).Result;

            var response = _mapper.Map<List<GetRoadStatusResponse>>(roadStatusList);

            return response;
        }
    }
}