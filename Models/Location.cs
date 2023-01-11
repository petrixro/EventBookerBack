using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace EventBookerBack.Models
{

    [Index(nameof(Name), IsUnique = true)]
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }


        public ICollection<Event>? Events { get; set; }

    }
}
