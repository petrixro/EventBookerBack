using EventBookerBack.Services;
using EventBookerBack.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventBookerBack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        public EventService _EventService;
        public EventController (EventService EventService)
        {
            _EventService = EventService;
        }

        [HttpGet("/Events")]
        public IActionResult getallEvents()
        {
            var _events = _EventService.getEvents();
            return Ok(_events);
        }

        [HttpGet("/Events/{id}")]
        public IActionResult GetEvent(int id)
        {
            var _event = _EventService.GetEventById(id);
            if (_event == null)
            {
                return NotFound("Event not found");
            }
            return Ok(_event);
        }

        [HttpGet("/Events/Location/{id}")]
        public IActionResult GetEventsByLocation(int id)
        {
            var _events = _EventService.getEventsByLocationId(id);
            return Ok(_events);
        }

        [HttpGet("/Events/User/{id}")]
        public IActionResult GetEventsByUser(int id)
        {
            var _events = _EventService.getEventsByUserAdminId(id);
            return Ok(_events);
        }

        [HttpPost("/Events")]
        public IActionResult AddEvent([FromBody] EventVM Event)
        {
            _EventService.AddEvent(Event);
            return Ok();
        }

        [HttpPost("/Events/Book/{eventId}/User/{userId}/TicketCount/{tickets}")]
        public IActionResult BookEvent(int eventId, int userId, int tickets)
        {
            var _event = _EventService.GetEventById(eventId);
            if (_EventService.BookTicket(eventId, userId, tickets)) return Ok($"You booked {tickets} tickets for Event: {_event.Name}");
            if (_event.Tickets - tickets < 0) return BadRequest("Not enough tickets left for this Event");
            return BadRequest("Your booking was not succesful!");
        }

        [HttpPut("/Events/{id}")]
        public IActionResult PutEvent([FromBody] EventVM Event, int id)
        {
            var _eventExists = _EventService.ModifyEvent(Event, id);
            if (_eventExists == null)
            {
                return NotFound("Event not found");
            }
            
            return Ok($"Event with id = {id} was successfuly modified.");
        }

        [HttpDelete("/Events/{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var _event = _EventService.GetEventById(id);
            if (_event == null)
            {
                return NotFound("Event not found");
            }
            _EventService.DeleteEvent(_event);
            return Ok($"Event with id {id} was deleted");
        }
    }
}
