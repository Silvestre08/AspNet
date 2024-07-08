using AutoMapper;
using CityInfo.Api.Entities;
using CityInfo.Api.Models;

namespace CityInfo.Api.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile() 
        {
            CreateMap<City, CityWithoutPointsOfInterestDto>();
            CreateMap<City, CityDto>();
        }
    }
}
