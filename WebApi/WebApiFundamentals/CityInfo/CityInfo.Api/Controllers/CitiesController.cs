using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using CityInfo.API;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities() 
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();
            
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            
            if (city == null) 
            {
                return NotFound();
            }

            return includePointsOfInterest 
                ? Ok(_mapper.Map<CityDto>(city)) 
                : Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}
