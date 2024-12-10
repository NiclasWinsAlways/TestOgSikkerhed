using System;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using BCrypt.Net;

namespace TestOgSikkerhed.Components.Account.Services
{
    public class HashService
    {
        public static string HashWithSHA2(object input)
        {
            byte[] inputBytes;

            // Determine input type and convert to byte array
            if (input is string inputString)
            {
                inputBytes = Encoding.UTF8.GetBytes(inputString);
            }
            else if (input is byte[] inputByteArray)
            {
                inputBytes = inputByteArray;
            }
            else
            {
                throw new ArgumentException("Input must be a string or byte array.");
            }

            // Compute the hash using SHA-256
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Return the hash as a Base64 string
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}