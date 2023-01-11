using EventBookerBack.Models;

namespace EventBookerBack.ViewModels
{
    public class UserLoginVM
    {
        public string password { get; set; }
        public string email { get; set; }
        public string? Role { get; set; }
    }
}
