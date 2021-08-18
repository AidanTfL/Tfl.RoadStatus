using System;
using System.Runtime.Serialization;
using AutoMapper;
using NUnit.Framework;
using TfL.RoadStatus.Application.GetRoadStatus;
using TfL.RoadStatus.Domain;

namespace TfL.RoadStatus.Application.UnitTests.RoadStatusProfileTests
{
    [TestOf(typeof(RoadStatusProfile))]
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoadStatusProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Test]
        [TestCase(typeof(Road), typeof(GetRoadStatusResponse))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type);

            return FormatterServices.GetUninitializedObject(type); // Type without parameterless constructor
        }
    }
}