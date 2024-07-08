using AutoMapper;
using CityInfo.Api.Entities;
using CityInfo.Api.Models;

namespace CityInfo.Api.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<PointOfInterest, PointOfInterestDto>();
        }
    }
}
