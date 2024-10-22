﻿using Asp.Versioning;
using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/cities")]
    [ApiVersion(1)]
    [ApiVersion(2)]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities([FromQuery]string? name, string? searchQuery, int pageNumber =1, int pageSize = 10) 
        {
            if (pageSize >= maxPageSize) 
            {
                pageSize = maxPageSize;
            }
            var (cityEntities, paginationMetadata) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageSize, pageNumber);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }


        /*
         * Attributes to help generating API documentation.
         * 
         */

        /// <summary>
        /// Get a city by id.
        /// </summary>
        /// <param name="cityId">The id of the city to get.</param>
        /// <param name="includePointsOfInterest">Whether or not to include the points of interest of the city.</param>
        /// <returns>A city with or without the points of interest.</returns>
        /// <response code="200">Returns the requested city.</response>
        [HttpGet("{cityId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCity(int cityId, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(cityId, includePointsOfInterest);
            
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
