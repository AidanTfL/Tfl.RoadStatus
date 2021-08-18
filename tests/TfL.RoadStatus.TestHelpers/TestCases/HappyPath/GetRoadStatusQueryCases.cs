using System;
using System.Collections.Generic;
using System.Linq;
using TfL.RoadStatus.Application.GetRoadStatus;

namespace TfL.RoadStatus.TestHelpers.TestCases
{
    public partial class GetRoadStatusQueryTestConstants
    {
        public class HappyPath
        {
            public class Get
            {
                public static IEnumerable<GetRoadStatusQuery> All()
                {
                    yield return EdgeCase.ValidWithSingleRoadId;
                    foreach (var query in GenerateValidWithMultipleRoadIds()) yield return query;
                    foreach (var query in GenerateValidWithSpaces()) yield return query;

                    yield return EdgeCase.ValidWithTwoIdenticalRoadIds;
                    yield return EdgeCase.ValidWithTwoIdenticalAnd1UniqueRoadIds;
                    yield return EdgeCase.ValidWithTwoIdenticalDifferentCaseRoadIds;
                    yield return EdgeCase.ValidWithTwoIdenticalDifferentCaseAnd1UniqueRoadIds;

                    yield return EdgeCase.ValidWithEmptyAppIdAndApiKey;
                }

                public static IEnumerable<GetRoadStatusQuery> GenerateValidWithMultipleRoadIds()
                {
                    var roadIds = new[] {"A1", "A2", "A3", "city route"};

                    for (var i = 2; i <= roadIds.Length; i++)
                        yield return new GetRoadStatusQuery
                        {
                            ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                            AppId = "ValidAppId",
                            ApiKey = "abcd1234566789",
                            RoadIds = roadIds.Take(i).ToArray()
                        };
                }

                public static IEnumerable<GetRoadStatusQuery> GenerateValidWithSpaces()
                {
                    yield return EdgeCase.ValidWithRoadIdThatHasSpace;
                    yield return EdgeCase.ValidWithTwoUniqueRoadIdsContainingSpaces;
                    yield return EdgeCase.ValidWithTwoIdenticalDifferentCaseContainingSpacesAnd1UniqueRoadIds;
                }
            }

            public class EdgeCase
            {
                internal static GetRoadStatusQuery ValidWithSingleRoadId => new GetRoadStatusQuery
                {
                    ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                    AppId = "ValidAppId",
                    ApiKey = "abcd1234566789",
                    RoadIds = new[] {"A1"}
                };

                internal static GetRoadStatusQuery ValidWithTwoIdenticalRoadIds => new GetRoadStatusQuery
                {
                    ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                    AppId = "ValidAppId",
                    ApiKey = "abcd1234566789",
                    RoadIds = new[] {"A1", "A1"}
                };

                internal static GetRoadStatusQuery ValidWithTwoIdenticalDifferentCaseRoadIds => new GetRoadStatusQuery
                {
                    ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                    AppId = "ValidAppId",
                    ApiKey = "abcd1234566789",
                    RoadIds = new[] {"A1", "a1"}
                };

                internal static GetRoadStatusQuery ValidWithTwoIdenticalAnd1UniqueRoadIds => new GetRoadStatusQuery
                {
                    ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                    AppId = "ValidAppId",
                    ApiKey = "abcd1234566789",
                    RoadIds = new[] {"A1", "A2", "A1"}
                };

                internal static GetRoadStatusQuery ValidWithTwoIdenticalDifferentCaseAnd1UniqueRoadIds =>
                    new GetRoadStatusQuery
                    {
                        ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                        AppId = "ValidAppId",
                        ApiKey = "abcd1234566789",
                        RoadIds = new[] {"a1", "A3", "A1"}
                    };

                internal static GetRoadStatusQuery ValidWithRoadIdThatHasSpace =>
                    new GetRoadStatusQuery
                    {
                        ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                        AppId = "ValidAppId",
                        ApiKey = "abcd1234566789",
                        RoadIds = new[] {"city route"}
                    };

                internal static GetRoadStatusQuery ValidWithTwoUniqueRoadIdsContainingSpaces =>
                    new GetRoadStatusQuery
                    {
                        ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                        AppId = "ValidAppId",
                        ApiKey = "abcd1234566789",
                        RoadIds = new[] {"city route", "inner ring"}
                    };

                internal static GetRoadStatusQuery
                    ValidWithTwoIdenticalDifferentCaseContainingSpacesAnd1UniqueRoadIds =>
                    new GetRoadStatusQuery
                    {
                        ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                        AppId = "ValidAppId",
                        ApiKey = "abcd1234566789",
                        RoadIds = new[] {"cITy rOUte", "City route", "A1"}
                    };

                internal static GetRoadStatusQuery ValidWithEmptyAppIdAndApiKey => new GetRoadStatusQuery
                {
                    ApiUri = new Uri("https://api.test.tfl.gov.uk"),
                    AppId = string.Empty,
                    ApiKey = string.Empty,
                    RoadIds = new[] {"A1"}
                };

                public static IEnumerable<GetRoadStatusQuery> GetValidWithSingleRoadId()
                {
                    yield return ValidWithSingleRoadId;
                }
            }
        }
    }
}