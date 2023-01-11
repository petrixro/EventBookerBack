using Microsoft.EntityFrameworkCore;

namespace EventBookerBack.Models
{
    [Index(nameof(email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string? phoneNumber { get; set; }
        public bool emailVerified { get; set; }
        public Role userRole { get; set; }
        public ICollection<Event>? Events { get; set; }
    }

    public enum Role
    {
        User,
        Admin,
        Company
    }
}
