using EventBookerBack.Data;
using EventBookerBack.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace EventBookerBack.Data
{
    public class DbInit
    {
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
        public static void Seed(IApplicationBuilder applicationBuilder ) 
        {
            using (var ServiceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = ServiceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (!context.Locations.Any())
                {
                    context.Locations.AddRangeAsync(new Location()
                    {
                        Name = "Location 1",
                        City = "Paris"
                    },
                    new Location()
                    {
                        Name = "Location 2",
                        City = "London"
                    });

                    context.SaveChanges();
                }
                if (!context.Users.Any())
                {
                    context.Users.Add(new User()
                    {
                        username= "admin",
                        email="admin@test",
                        emailVerified=true,
                        userRole = Role.Admin,
                        password= EncryptPassword("admin")
                    });

                    context.SaveChanges();
                }

            }
        }
    }
}
