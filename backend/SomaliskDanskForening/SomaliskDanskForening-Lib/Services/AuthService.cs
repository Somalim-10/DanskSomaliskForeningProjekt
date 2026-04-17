// backend/SomaliskDanskForening/SomaliskDanskForening-Lib/Services/AuthService.cs
using System.Security.Cryptography;
using System.Text;

namespace SomaliskDanskForening_Lib.Services
{
    public class AuthService
    {
        // Hash password med salt
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var salt = new byte[16];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);

                byte[] hashWithSalt = new byte[36];
                Array.Copy(salt, 0, hashWithSalt, 0, 16);
                Array.Copy(hash, 0, hashWithSalt, 16, 20);

                return Convert.ToBase64String(hashWithSalt);
            }
        }

        // Verify password
        public static bool VerifyPassword(string password, string hash)
        {
            byte[] hashWithSalt = Convert.FromBase64String(hash);
            byte[] salt = new byte[16];
            Array.Copy(hashWithSalt, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] computedHash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashWithSalt[i + 16] != computedHash[i])
                    return false;
            }
            return true;
        }
    }
}