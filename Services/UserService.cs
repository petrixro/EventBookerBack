using EventBookerBack.Models;
using EventBookerBack.ViewModels;
using Microsoft.EntityFrameworkCore;
using EventBookerBack.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Net.Mail;

namespace EventBookerBack.Services
{
    public class UserService
    {
        private ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static string EncryptPassword(string password)
        {
            // Encrypt password
            byte[] salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG

            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return encryptedPassw;
        }

        public void SendEmail(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("From Email");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("TEST@TEST", "password");
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true ;
            client.Port = 587;

            client.Send(mailMessage);
        }

        public void AddUser(UserLoginVM newUser)
        {
            var newUserRole = Role.User;
            if (newUser.Role.Equals("Admin")) 
            { 
                newUserRole = Role.Admin;
            } else if (newUser.Role.Equals("User"))
            {
                newUserRole = Role.User;
            } else 
            { 
                newUserRole = Role.Company; 
            }

            var _User = new User()
            {
                username = newUser.email,
                password = EncryptPassword(newUser.password),
                email = newUser.email,
                emailVerified = false,
                userRole = newUserRole
            };
            var _ConfirmationLink = $"To confirm your user account on EventBooker access this link: /token={_User.email}";
            if (_User.userRole.Equals(Role.User))
                SendEmail(_User.email, _ConfirmationLink);
            _context.Users.Add(_User);
            _context.SaveChanges();
 
        }

        public bool VerifyUserAccount(string email)
        {
            var currentUser = _context.Users.FirstOrDefault(x => x.email.ToLower().Equals(email.ToLower()));
            if (currentUser != null)
            {
                currentUser.emailVerified = true;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public User Authenticate(UserLoginVM userLogin)
        {
            var currentUser = _context.Users.FirstOrDefault(x => x.email.ToLower().Equals(userLogin.email.ToLower())
                && EncryptPassword(userLogin.password).Equals(x.password));
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }

        public String EmailAlreadyRegistered(UserLoginVM userLogin)
        {
            var currentUser = _context.Users.FirstOrDefault(x => x.email.ToLower().Equals(userLogin.email.ToLower()));
            if (currentUser != null)
            {
                return "User email already registered";
            }
            return "OK";
        }
    }
    
} 

