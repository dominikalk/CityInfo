using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
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
            CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            PointOfInterestDto? pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDto pointOfInterestDto)
        {
            CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            int maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

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
            CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            PointOfInterestDto? pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);

            if (pointOfInterestFromStore == null)
            {
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
            CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            PointOfInterestDto? pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
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
            CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            PointOfInterestDto? pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterest);
            return NoContent();
        }
    }
}
