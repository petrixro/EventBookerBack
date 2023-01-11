using EventBookerBack.Data;
using EventBookerBack.Models;
using EventBookerBack.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EventBookerBack.Services
{
    public class LocationService
    {
        private ApplicationDbContext _context;
        public LocationService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public void AddLocation (LocationVM location)
        {
            var _location = new Location()
            {
                Name= location.Name,
                City= location.City
            };
            _context.Locations.Add(_location);
            _context.SaveChanges();
        }

        public bool ModifyLocation(LocationVM location, int id)
        {
            var _location = _context.Locations.FirstOrDefault(x => x.Id == id);
            if (_location != null)
            {
                _location.City = location.City;
                _location.Name= location.Name;
                _context.SaveChanges();
                return true;
            }
            return false;

        }

        public List<Location> GetLocations()  => _context.Locations.Include("Events").ToList();
        public Location GetLocationById(int id) => _context.Locations.Include("Events").FirstOrDefault(l => l.Id == id);
        public List<Location> GetLocationsByCity(string city) => _context.Locations.Include("Events").Where(l => l.City.Equals(city)).ToList();

        public List<Location> GetLocationsByName(string name) => _context.Locations.Include("Events").Where(l => l.Name.Contains(name)).ToList();

        public void DeleteLocation (Location location)
        {
            _context.Locations.Remove(location);
            _context.SaveChanges();
        }
    }
}
