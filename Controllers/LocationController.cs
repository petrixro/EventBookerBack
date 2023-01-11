using EventBookerBack.Services;
using EventBookerBack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookerBack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        public LocationService _locationService;
        public LocationController (LocationService locationService)
        {
            _locationService = locationService;
        }

        
        [HttpGet("/Locations")]
        public IActionResult GetAllLocations()
        {
            var _locations = _locationService.GetLocations();
            return Ok(_locations);
        }

        [HttpGet("/Locations/{id}")]
        public IActionResult GetLocation(int id) 
        { 
            var location = _locationService.GetLocationById(id);
            if (location == null)
            {
                return NotFound("Location not found");
            }
            return Ok(location);
        }

        [HttpGet("/Locations/City/{city}")]
        public IActionResult GetLocationsByCity(string city)
        {
            var _locations = _locationService.GetLocationsByCity(city);
            return Ok(_locations);
        }

        [HttpGet("/Locations/Name/{name}")]
        public IActionResult GetLocationsByName(string name)
        {
            var _locations = _locationService.GetLocationsByName(name);
            return Ok(_locations);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/Locations")]
        public IActionResult AddLocation([FromBody] LocationVM location)
        {
            _locationService.AddLocation(location);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/Locations/{id}")]
        public IActionResult UpdateLocation([FromBody] LocationVM location, int id)
        {
            var _location = _locationService.ModifyLocation(location, id);
            if (_location == false)
            {
                return NotFound("Location not found");
            }
            return Ok($"Location with id = {id} was successfully updated!");
        }

        [HttpDelete("/Locations/{id}")]
        public IActionResult DeleteLocation(int id)
        {
            var location = _locationService.GetLocationById(id);
            if (location == null)
            {
                return NotFound("Location not found");
            }
            _locationService.DeleteLocation(location);
            return Ok($"Location with id {id} was deleted");
        }
    }
}
