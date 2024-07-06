using CityInfo.Api.Models;
using CityInfo.API;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, CitiesDataStore citiesDataStore)
        {
            _logger = logger;
            _mailService = mailService;
            _citiesDataStore = citiesDataStore;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestCreateDto>> GetPoinstOfInterest(int cityId)
        {
            var city = GetCityDto(cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = GetCityDto(cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id {cityId} was not found.");
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.SingleOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        //the attribute from body is not necessary because we are using the api controller attribute at the controller level

        //        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId,[FromBody] Poi0ntOfInterestCreateDto pointOfInterestCreateDto) 
        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestCreateDto pointOfInterestCreateDto)
        {
            var city = GetCityDto(cityId);

            if (city == null)
            {
                return NotFound();
            }
            var id = city.PointsOfInterest.Max(P => P.Id) + 1;

            var pointOfInterest = new PointOfInterestDto { Id = id, Description = pointOfInterestCreateDto.Description, Name = pointOfInterestCreateDto.Name };
            city.PointsOfInterest.Add(pointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId,
                pointOfInterestId = pointOfInterest.Id,
            }, pointOfInterest);

        }

        [HttpPut("{pointofinterestid}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestUpdateDto pointOfInterestUpdateDto)
        {
            var city = GetCityDto(cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            pointOfInterest.Name = pointOfInterestUpdateDto.Name;
            pointOfInterest.Description = pointOfInterestUpdateDto.Description;

            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestUpdateDto> jsonPatchDocument)
        {
            var city = GetCityDto(cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestUpdateDto()
            {
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };

            jsonPatchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch)) 
            {
                return BadRequest(ModelState);
            }

            pointOfInterest.Name = pointOfInterestToPatch.Name;
            pointOfInterest.Description = pointOfInterestToPatch.Description;

            return NoContent();

        }

        [HttpDelete("{pointofinterestid}")]
        public ActionResult deletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = GetCityDto(cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterest);
            _mailService.Send("Point of interest deleted", $"Point of interest {pointOfInterest.Name} with id {pointOfInterest.Id} has been deleted");
            return NoContent();
        }

        private CityDto? GetCityDto(int cityId)
        {
            return _citiesDataStore.Cities.SingleOrDefault(c => c.Id == cityId);
        }
    }
}
