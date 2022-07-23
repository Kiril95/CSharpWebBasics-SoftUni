using FirstMvcApp.Data;
using FirstMvcApp.Models;
using FirstMvcApp.ViewModels;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace FirstMvcApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext db;

        public UserService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public string GetUserId(string username, string password)
        {
            // If we don't find a correct user return null
            var hashedPassword = ComputeHash(password);
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            return user?.Id;
        }

        public string Create(RegisterInputModel input)
        {
            if (input.Username.Length < 5 || input.Username.Length > 20)
            {
                throw new ArgumentException("Username must be between 5 and 20 characters long.");
            }

            var isMatch = Regex.Match(input.Email, @"^([^@\s]+@[^@\s]+\.(com|bg|net|org|gov))$");
            if (!isMatch.Success)
            {
                throw new ArgumentException("Email is not valid.");
            }
            if (input.Password.Length < 6 || input.Password.Length > 20)
            {
                throw new ArgumentException("Password must be between 6 and 20 characters long.");
            }
            if (input.Password != input.ConfirmPassword)
            {
                throw new ArgumentException("Passwords do not match.");
            }

            User user = new User
            {
                Username = input.Username,
                Password = ComputeHash(input.Password),
                Email = input.Email,
            };

            db.Users.Add(user);
            db.SaveChanges();

            return user.Id;
        }

        private static string ComputeHash(string inputPassword)
        {
            var bytes = Encoding.UTF8.GetBytes(inputPassword);
            using var hash = SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);
            // Convert to text
            // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 

            var hashedInputStringBuilder = new StringBuilder(128);
            foreach (var b in hashedInputBytes)
            {
                hashedInputStringBuilder.Append(b.ToString("X2"));
            }

            return hashedInputStringBuilder.ToString();
        }
    }
}
