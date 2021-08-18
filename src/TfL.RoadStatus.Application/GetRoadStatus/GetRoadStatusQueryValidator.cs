using FluentValidation;

namespace TfL.RoadStatus.Application.GetRoadStatus
{
    public class GetRoadStatusQueryValidator : AbstractValidator<GetRoadStatusQuery>
    {
        public GetRoadStatusQueryValidator()
        {
            RuleFor(x => x.RoadIds)
                .NotEmpty().WithMessage(x => $"One or more {nameof(x.RoadIds)} are required")
                .Must(x => x?.Length <= 4).WithMessage(x => $"Please specify up to 4 {nameof(x.RoadIds)} only");

            RuleFor(x => x.ApiUri)
                .NotEmpty().WithMessage(x => $"An {nameof(x.ApiUri)} is required");

            RuleFor(x => x.AppId)
                .NotNull().WithMessage(x => $"{nameof(x.AppId)} is not required, but should be specified as empty");

            RuleFor(x => x.ApiKey)
                .NotNull().WithMessage(x => $"An {nameof(x.ApiKey)} is not required, but should be specified as empty");
        }
    }
}