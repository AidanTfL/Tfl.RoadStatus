using System;
using System.Collections.Generic;
using TfL.RoadStatus.Application.GetRoadStatus;

namespace TfL.RoadStatus.TestHelpers.TestCases
{
    public partial class GetRoadStatusQueryTestConstants
    {
        public class UnhappyPath
        {
            public class Get
            {
                public static IEnumerable<GetRoadStatusQuery> All()
                {
                    foreach (var invalidQuery in GenerateWithEmptyStringProperty()) yield return invalidQuery;
                    foreach (var invalidQuery in GenerateWithNullProperty()) yield return invalidQuery;

                    yield return EdgeCase.InvalidEmptyRoadIdsAndApiKey;
                    yield return EdgeCase.InvalidEmptyRoadIdsAndWhitespaceApiKey;
                }

                public static IEnumerable<GetRoadStatusQuery> GenerateWithEmptyStringProperty()
                {
                    var queries = new List<GetRoadStatusQuery>();
                    foreach (var prop in typeof(GetRoadStatusQuery).GetProperties())
                    {
                        if (prop.Name == nameof(GetRoadStatusQuery.AppId)) continue; //AppId == empty is happy path
                        if (prop.Name == nameof(GetRoadStatusQuery.ApiKey)) continue; //ApiKey == empty is happy path

                        var query = HappyPath.EdgeCase.ValidWithSingleRoadId;
                        try
                        {
                            prop.SetValue(query, string.Empty);
                            queries.Add(query);
                        }
                        catch { /*continue*/ }
                    }

                    return queries;
                }

                public static IEnumerable<GetRoadStatusQuery> GenerateWithNullProperty()
                {
                    var queries = new List<GetRoadStatusQuery>();
                    foreach (var prop in typeof(GetRoadStatusQuery).GetProperties())
                    {
                        if (prop.Name == nameof(GetRoadStatusQuery.AppId)) continue;
                        if (prop.Name == nameof(GetRoadStatusQuery.ApiKey)) continue;
                        var query = HappyPath.EdgeCase.ValidWithSingleRoadId;
                        try
                        {
                            prop.SetValue(query, null);
                            queries.Add(query);
                        }
                        catch { /*continue*/ }
                    }

                    return queries;
                }
            }

            public static class EdgeCase
            {
                internal static GetRoadStatusQuery InvalidEmptyRoadIdsAndApiKey => new GetRoadStatusQuery
                {
                    ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                    AppId = "ValidAppId",
                    ApiKey = string.Empty,
                    RoadIds = Array.Empty<string>()
                };

                internal static GetRoadStatusQuery InvalidEmptyRoadIdsAndWhitespaceApiKey => new GetRoadStatusQuery
                {
                    ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                    AppId = "ValidAppId",
                    ApiKey = " ",
                    RoadIds = Array.Empty<string>()
                };

                internal static GetRoadStatusQuery NotFoundRoadId => new GetRoadStatusQuery
                {
                    ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                    AppId = "ValidAppId",
                    ApiKey = "abcd1234566789",
                    RoadIds = new[] {"RoadWithoutTraffic"}
                };

                public static IEnumerable<GetRoadStatusQuery> GetNotFoundRoadId()
                {
                    yield return NotFoundRoadId;
                }
            }
        }
    }
}