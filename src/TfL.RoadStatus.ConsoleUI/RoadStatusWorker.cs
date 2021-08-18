using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TfL.RoadStatus.Application;
using TfL.RoadStatus.Application.GetRoadStatus;
using static System.Console;

namespace TfL.RoadStatus.ConsoleUI
{
    public class RoadStatusWorker : BackgroundService
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly AppConfig _config;
        private readonly ISender _mediator;

        public RoadStatusWorker(ISender mediator, IOptions<AppConfig> appConfig,
            IHostApplicationLifetime applicationLifetime)
        {
            _config = appConfig.Value;
            _mediator = mediator;
            _applicationLifetime = applicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var request = new GetRoadStatusQuery
            {
                RoadIds = _config.RoadIds,
                ApiUri = _config.ApiUrl,
                AppId = _config.AppId,
                ApiKey = _config.ApiKey
            };

            var response = await _mediator.Send(request, stoppingToken);

            OutputResponse(response);

            _applicationLifetime.StopApplication();
        }

        protected void OutputResponse(IList<GetRoadStatusResponse> response)
        {
            foreach (var roadStatus in response)
            {
                WriteLine($"The status of the {roadStatus.DisplayName} is as follows");
                WriteLine($"\t Road Status is {roadStatus.RawStatusSeverity}");
                WriteLine($"\t Road Status Description is {roadStatus.StatusSeverityDescription}\n");
            }
        }
    }
}