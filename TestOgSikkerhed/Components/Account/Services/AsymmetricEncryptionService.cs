using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TestOgSikkerhed.Components.Account.Services
{
    public class AsymmetricEncryptionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _encryptionApiBaseUrl;
        private readonly string _publicKeyPath = "publicKey.xml";
        private readonly string _privateKeyPath = "privateKey.xml";
        private readonly string _publicKey;
        private readonly string _privateKey;

        public AsymmetricEncryptionService()
        {
            _httpClient = new HttpClient();
            _encryptionApiBaseUrl = "https://localhost:7009"; // Swagger API base URL

            // Load or generate key pair
            if (File.Exists(_publicKeyPath) && File.Exists(_privateKeyPath))
            {
                // Load keys from files
                _publicKey = File.ReadAllText(_publicKeyPath);
                _privateKey = File.ReadAllText(_privateKeyPath);
            }
            else
            {
                // Generate new keys and save them to files
                using (var rsa = new RSACryptoServiceProvider(2048))
                {
                    _privateKey = rsa.ToXmlString(true);  // Includes private key
                    _publicKey = rsa.ToXmlString(false); // Public key only

                    File.WriteAllText(_privateKeyPath, _privateKey);
                    File.WriteAllText(_publicKeyPath, _publicKey);
                }
            }
        }

        public string GetPublicKey()
        {
            return _publicKey; // Return the public key
        }

        public string GetPrivateKey()
        {
            return _privateKey; // Return the private key
        }

        public async Task<string> EncryptAsync(string valueToEncrypt)
        {
            if (string.IsNullOrEmpty(valueToEncrypt))
            {
                throw new ArgumentNullException(nameof(valueToEncrypt), "Value to encrypt cannot be null or empty.");
            }

            try
            {
                // Perform encryption using the public key
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(_publicKey); // Load the public key

                    byte[] valueBytes = Encoding.UTF8.GetBytes(valueToEncrypt);
                    byte[] encryptedBytes = rsa.Encrypt(valueBytes, false); // Use PKCS#1 v1.5 padding

                    return Convert.ToBase64String(encryptedBytes); // Return encrypted text as Base64
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Encryption failed: {ex.Message}");
            }
        }

        public async Task<string> DecryptAsync(string valueToDecrypt)
        {
            if (string.IsNullOrEmpty(valueToDecrypt))
            {
                throw new ArgumentNullException(nameof(valueToDecrypt), "Value to decrypt cannot be null or empty.");
            }

            try
            {
                // Perform decryption using the private key
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(_privateKey); // Load the private key

                    byte[] encryptedBytes = Convert.FromBase64String(valueToDecrypt);
                    byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);

                    return Encoding.UTF8.GetString(decryptedBytes); // Convert decrypted bytes back to string
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Decryption failed: {ex.Message}");
            }
        }
    }
}