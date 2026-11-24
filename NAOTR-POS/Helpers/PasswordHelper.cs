
using System;
using System.Security.Cryptography;
using System.Text;

namespace NAOTR_POS.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(
            string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(
                    salt + password);

                byte[] hash = sha256.ComputeHash(bytes);

                return BitConverter.ToString(hash)
                    .Replace("-", "");
            }
        }

        public static bool VerifyPassword(
            string password,
            string salt,
            string storedHash)
        {
            string computedHash =
                HashPassword(password, salt);

            return string.Equals(
                computedHash,
                storedHash,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
