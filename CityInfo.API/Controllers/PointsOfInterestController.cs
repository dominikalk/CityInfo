using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            CityDto? city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id {cityId} wasn't found.");
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            CityDto? city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id {cityId} wasn't found.");
                return NotFound();
            }

            PointOfInterestDto? pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                _logger.LogInformation($"Point of interest with id {pointOfInterestId} wasn't found.");
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDto pointOfInterestDto)
        {
            CityDto? city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id {cityId} wasn't found.");
                return NotFound();
            }

            int maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            PointOfInterestDto newPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterestDto.Name,
                Description = pointOfInterestDto.Description,

            };

            city.PointsOfInterest.Add(newPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId,
                pointOfInterestId = newPointOfInterest.Id
            }, newPointOfInterest);
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterestDto)
        {
            CityDto? city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id {cityId} wasn't found.");
                return NotFound();
            }

            PointOfInterestDto? pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);

            if (pointOfInterestFromStore == null)
            {
                _logger.LogInformation($"Point of interest with id {pointOfInterestId} wasn't found.");
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterestDto.Name;
            pointOfInterestFromStore.Description = pointOfInterestDto.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            CityDto? city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id {cityId} wasn't found.");
                return NotFound();
            }

            PointOfInterestDto? pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                _logger.LogInformation($"Point of interest with id {pointOfInterestId} wasn't found.");
                return NotFound();
            }

            PointOfInterestForUpdateDto pointOfInterestToPatch =
                new PointOfInterestForUpdateDto()
                {
                    Name = pointOfInterest.Name,
                    Description = pointOfInterest.Description,
                };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

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

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            CityDto? city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id {cityId} wasn't found.");
                return NotFound();
            }

            PointOfInterestDto? pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                _logger.LogInformation($"Point of interest with id {pointOfInterestId} wasn't found.");
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterest);
            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterest.Name} with id {pointOfInterest.Id} has been deleted.");
            return NoContent();
        }
    }
}
