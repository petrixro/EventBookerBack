using EventBookerBack.Data;
using EventBookerBack.Models;
using EventBookerBack.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EventBookerBack.Services
{
    public class EventService
    {
        private ApplicationDbContext _context;
        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddEvent(EventVM newEvent)
        {
            var _location = _context.Locations.Find(newEvent.LocationId);
            var _user = _context.Users.FirstOrDefault(u => u.email.ToLower() == newEvent.UserEmail.ToLower());

            var _event = new Event()
            {
                Name = newEvent.Name,
                Date = newEvent.Date,
                Tickets = newEvent.Tickets,
                Location = _location,
                LocationId = newEvent.LocationId,
                UserAdmin = _user,
                UserId = _user.Id
            };
            _context.Events.Add(_event);
            _context.SaveChanges();
 
        }

        public EventVM ModifyEvent(EventVM newEvent, int id) 
        {
            var _event = _context.Events.FirstOrDefault(x => x.Id == id);
            if (_event != null)
            {
                var _location = _context.Locations.FirstOrDefault(x => x.Id == newEvent.LocationId);
                _event.Location = _location;
                _event.LocationId = _location.Id;
                var _user = _context.Users.FirstOrDefault(x => x.email.ToLower().Equals(newEvent.UserEmail.ToLower()));
                _event.UserAdmin = _user;
                _event.UserId = _user.Id;
                _event.Tickets = newEvent.Tickets;
                _event.Date = newEvent.Date;
                _event.Name = newEvent.Name;
                _context.SaveChanges();
                return newEvent;
            }
            return null;
           
        }

        public List<Event> getEvents() => _context.Events.ToList();
        public Event GetEventById(int id) => _context.Events.Include("Location").FirstOrDefault(e => e.Id == id);
        public List<Event> getEventsByLocationId(int id) => _context.Events.Where(e => e.LocationId== id).ToList();
        public List<Event> getEventsByUserAdminId(int id) => _context.Events.Where(e => e.UserId == id).ToList();
        public void DeleteEvent(Event _event)
        {
            _context.Events.Remove(_event);
            _context.SaveChanges();
        }

        public bool BookTicket(int eventid, int userid, int tickets)
        {
            var _user = _context.Users.FirstOrDefault(u => u.Id == userid);
            var _event = _context.Events.FirstOrDefault(e => e.Id == eventid);
            if (_event != null && _user != null)
            {
                if (_event.Tickets - tickets < 0) return false;
                _event.Tickets -= tickets;
                for (int i = 0; i < tickets; i++)
                {
                    var _ticket = new Ticket()
                    {
                        EventId = _event.Id,
                        UserId = _user.Id
                    };
                    _context.Tickets.Add(_ticket);
                }
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
    
} 

