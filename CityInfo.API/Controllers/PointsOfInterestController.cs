using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            CityDto? cityDto = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            if (cityDto == null)
            {
                return NotFound();
            }

            return Ok(cityDto.PointsOfInterest);
        }

        [HttpGet("{pointOfInterestId}")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            CityDto? cityDto = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            if (cityDto == null)
            {
                return NotFound();
            }

            PointOfInterestDto? pointOfInterestDto = cityDto.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);

            if (pointOfInterestDto == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterestDto);
        }
    }
}
