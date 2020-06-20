using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace Payment.Gateway.Application.Encryption
{
    public class AesCryptoService : IAesCryptoService
    {
        private readonly IKeyGenerator _keyGenerator;
        private readonly ILogger<AesCryptoService> _logger;

        public AesCryptoService(IKeyGenerator keyGenerator, ILogger<AesCryptoService> logger)
        {
            _keyGenerator = keyGenerator ?? throw new ArgumentNullException(nameof(keyGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public string Encrypt(string text, string key, byte[] IV = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (key == null) throw new ArgumentNullException(nameof(text));
                
            try
            {
                return EncryptString(text, key, IV);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,exception.Message);
                throw new EncryptException(exception);
            }
        }

        private string EncryptString(string text, string key, byte[] IV = null)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.KeySize = _keyGenerator.GetKeySize();
                var keyBytes = _keyGenerator.Generate(key);
                var vector = IV ?? aesAlg.IV;
                
                var encryptor = aesAlg.CreateEncryptor(keyBytes, vector);

                using var msEncrypt = new MemoryStream();
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(text);
                }

                var encrypted = msEncrypt.ToArray();

                var result = new byte[vector.Length + encrypted.Length];

                Buffer.BlockCopy(vector, 0, result, 0, vector.Length);
                Buffer.BlockCopy(encrypted, 0, result, vector.Length, encrypted.Length);

                return Convert.ToBase64String(result);
            }
        }
        
        public string Decrypt(string cipherText, string key, byte[] IV = null)
        {
            if (cipherText == null) throw new ArgumentNullException(nameof(cipherText));
            if (key == null) throw new ArgumentNullException(nameof(key));
            
            try
            {
                return DecryptString(cipherText, key, IV);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,exception.Message);
                throw new DecryptException(exception);
            }
        }

        private string DecryptString(string cipherText, string key, byte[] IV = null)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var vector = IV ?? new byte[16];
            var cipher = new byte[fullCipher.Length - vector.Length];

            Buffer.BlockCopy(fullCipher, 0, vector, 0, vector.Length);
            Buffer.BlockCopy(fullCipher, vector.Length, cipher, 0, cipher.Length);

            using var aes = Aes.Create();
            aes.KeySize = _keyGenerator.GetKeySize();
            aes.IV = vector;

            var keyBytes = _keyGenerator.Generate(key);

            using var decryptor = aes.CreateDecryptor(keyBytes, vector);
            using var memoryStream = new MemoryStream(cipher);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            var result = streamReader.ReadToEnd();

            return result;
        }
    }
}