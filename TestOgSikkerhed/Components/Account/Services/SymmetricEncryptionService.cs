using Microsoft.AspNetCore.DataProtection;

namespace TestOgSikkerhed.Components.Account.Services
{
    public class SymmetricEncryptionService
    {
        private readonly IDataProtector _protector;

        public SymmetricEncryptionService(IDataProtectionProvider dataProtectionProvider)
        {
            if (dataProtectionProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProtectionProvider), "DataProtectionProvider cannot be null.");
            }

            // Create a protector with a unique purpose
            _protector = dataProtectionProvider.CreateProtector("SymmetricEncryptionServicePurpose");
        }
        public string Encrypt(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
            {
                throw new ArgumentNullException(nameof(plaintext), "Value to encrypt cannot be null or empty.");
            }

            return _protector.Protect(plaintext);
        }
        public string Decrypt(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
            {
                throw new ArgumentNullException(nameof(ciphertext), "Value to decrypt cannot be null or empty.");
            }

            return _protector.Unprotect(ciphertext);
        }
    }
}
