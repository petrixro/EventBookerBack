using EventBookerBack.Models;
using EventBookerBack.Services;
using EventBookerBack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventBookerBack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserService _UserService;

        private readonly IConfiguration _config;

        public UserController (UserService UserService, IConfiguration Configuration)
        {
            _UserService = UserService;
            _config = Configuration;
        }

        // To generate token
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.username),
                new Claim(ClaimTypes.Role, user.userRole.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            System.Console.Write(token.Claims.ToString());

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [HttpGet("/ActivateAccount/token={email}")]
        public IActionResult ValidateUserAccount(string email)
        {
            var CheckValidation = _UserService.VerifyUserAccount(email);
            if (CheckValidation)
                return Ok("Account activated");
            return BadRequest("Account activation failed");

        }

        [HttpPost("register")]
        public IActionResult AddUser([FromBody] UserLoginVM User)
        {
            var CheckEmail = _UserService.EmailAlreadyRegistered(User);
            if (CheckEmail == "OK") {
                _UserService.AddUser(User);
                return Ok(User);
            }
            return BadRequest("Email already registered.");
            
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLoginVM userLogin)
        {
            var user = _UserService.Authenticate(userLogin);
            if (user != null && user.emailVerified.Equals(true))
            {
                var token = GenerateToken(user);
                var _newUserAuth = new UserAuth()
                {
                    Id = user.Id,
                    Username = user.username,
                    Email = user.email,
                    JWT = token
                };
                return Ok(_newUserAuth);
            }
            else if (user.emailVerified.Equals(false)) return NotFound("User account is not verified.");
            {
                return NotFound("User not found or incorrect password.");
            }
        }
    }
}
