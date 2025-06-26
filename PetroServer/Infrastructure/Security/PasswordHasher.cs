using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security
{
    public static class PasswordHasher
    {
        public static List<string> Hash(string password)
        {
            // Tạo salt (padding)
            var salt = Guid.NewGuid().ToString("N").Substring(0, 8);
            var combinedPassword = password + salt;

            // Băm chuỗi
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(combinedPassword);
            var hashBytes = sha256.ComputeHash(bytes);
            var hashedPassword = Convert.ToBase64String(hashBytes);

            return new List<string> { hashedPassword, salt };
        }

        public static bool Verify(string inputPassword, string storedHash, string salt)
        {
            var combined = inputPassword + salt;
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(combined);
            var hashBytes = sha256.ComputeHash(bytes);
            var inputHash = Convert.ToBase64String(hashBytes);

            return inputHash == storedHash;
        }
    }
}
