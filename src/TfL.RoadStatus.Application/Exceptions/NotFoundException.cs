using System;
using System.Collections.Generic;
using System.Linq;

namespace TfL.RoadStatus.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Type entityType, IReadOnlyCollection<string> value)
            : base(value.Count > 1
                ? $"{string.Join(" ", value)} are not valid {entityType.Name.ToLower()}s"
                : $"{value.First()} is not a valid {entityType.Name.ToLower()}")
        {
        }
    }
}