using FirstMvcApp.Data;
using FirstMvcApp.Models;
using FirstMvcApp.ViewModels;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
            var hashedPassword = ComputeHash(password);
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            return user?.Id;
        }

        public string Create(RegisterInputModel input)
        {
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
