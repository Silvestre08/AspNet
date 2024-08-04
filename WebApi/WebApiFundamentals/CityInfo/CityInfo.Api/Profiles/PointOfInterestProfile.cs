using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Domain.Entities;

namespace CityInfo.Infrastructure.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<PointOfInterest, PointOfInterestDto>(); 
            CreateMap<PointOfInterestUpdateDto, PointOfInterest>();
            CreateMap<PointOfInterest, PointOfInterestUpdateDto>();
            CreateMap<PointOfInterestCreateDto, PointOfInterest>();
        }
    }
}
