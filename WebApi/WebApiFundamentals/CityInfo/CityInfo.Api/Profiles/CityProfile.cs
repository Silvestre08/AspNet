using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Domain.Entities;

namespace CityInfo.Infrastructure.Profiles
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
