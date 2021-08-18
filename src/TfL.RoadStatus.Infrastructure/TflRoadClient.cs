using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using TfL.RoadStatus.Application.Exceptions;
using TfL.RoadStatus.Application.GetRoadStatus;
using TfL.RoadStatus.Application.Interfaces;
using TfL.RoadStatus.Domain;

namespace TfL.RoadStatus.Infrastructure
{
    //todo: consider implementing the NSwag auto-generated IRoadClient interface instead

    public class TflRoadClient : IRoadClient
    {
        private readonly IFlurlClient _client;
        private readonly IUrlBuilder _urlBuilder;

        public TflRoadClient(IUrlBuilder urlBuilder, IFlurlClientFactory httpClientFactory)
        {
            _urlBuilder = urlBuilder;
            _client = httpClientFactory.Get(nameof(TflRoadClient));
        }

        public async Task<IList<Road>> GetRoadStatusAsync(GetRoadStatusQuery roadStatusQuery)
        {
            var requestUrl = _urlBuilder.BuildFrom(roadStatusQuery).AbsoluteUri;

            try
            {
                return await _client.Request(requestUrl).WithTimeout(12).GetJsonAsync<List<Road>>();
            }
            catch (FlurlHttpTimeoutException ex)
            {
                throw new ApiClientException(customMessage: "Timeout: Please check your internet connection", innerException: ex);
            }
            catch (FlurlHttpException ex)
            {
                if (ex.StatusCode == null) throw new ApiClientException(customMessage: ex.InnerException?.Message);

                var errorStatusCode = (HttpStatusCode) ex.StatusCode;
                var errorResponse = await ex.GetResponseStringAsync();

                switch (errorStatusCode)
                {
                    case HttpStatusCode.NotFound:
                    {
                        var apiError = JsonConvert.DeserializeObject<ApiError>(errorResponse);

                        var roadIdsNotFound = roadStatusQuery.RoadIds
                            .Where(r => apiError != null && apiError.Message.Contains(r)).ToList();

                        throw new NotFoundException(typeof(Road), roadIdsNotFound);
                    }
                    case HttpStatusCode.TooManyRequests:
                    {
                        throw new ApiClientException(errorStatusCode, ex,
                            customMessage: errorResponse); //invalid api_key
                    }
                    case HttpStatusCode.BadRequest:
                    {
                        try
                        {
                            var apiError = JsonConvert.DeserializeObject<ApiError>(errorResponse);
                            throw new ApiClientException(errorStatusCode, ex, apiError);
                        }
                        catch (JsonSerializationException)
                        {
                            throw new ApiClientException(errorStatusCode, ex);
                        }
                    }
                    default:
                    {
                        throw new ApiClientException(errorStatusCode);
                    }
                }
            }
        }
    }
}