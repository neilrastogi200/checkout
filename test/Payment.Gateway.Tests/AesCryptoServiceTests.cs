using System;
using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Payment.Gateway.Application.Encryption;
using Xunit;

namespace Payment.Gateway.Tests
{
    public class AesCryptoServiceTests
    {
        private readonly Mock<ILogger<AesCryptoService>> _mockLogger;

        public AesCryptoServiceTests()
        {
            _mockLogger = new Mock<ILogger<AesCryptoService>>();
        }

        [Theory]
        [InlineData("Test encryption", "JGUR465DY")]
        [InlineData("124364745859", "")]
        [InlineData("{ data: { password: '123456', username: 'admin', name: 'John', lastname: 'Smith', salt: '36dvd373vdgd838dbdb8364gdvd73gdb37db37dbef' } }", "CFY%DG&RF*&FVHY65dFH76FGH7")]
        public void Encrypt_and_Decrypt_string_with_256_bits_key_encryption(string plainText, string key)
        {   
            var aes = new AesCryptoService(new Key256BitsGenerator(), _mockLogger.Object);
            
            var encryptedText = aes.Encrypt(plainText, key);
            var decryptedText = aes.Decrypt(encryptedText, key);

            decryptedText.Should().Be(plainText);
        }
        
        [Theory]
        [InlineData("Test encryption", "JGUR465DY")]
        [InlineData("124364745859", "")]
        [InlineData("{ data: { password: '123456', username: 'admin', name: 'John', lastname: 'Smith', salt: '36dvd373vdgd838dbdb8364gdvd73gdb37db37dbef' } }", "CFY%DG&RF*&FVHY65dFH76FGH7")]
        public void Encrypt_and_Decrypt_string_with_256_bit_key_encryption_and_custom_vector(string plainText, string key)
        {
            using (var myAes = Aes.Create())
            {
                var aes = new AesCryptoService(new Key256BitsGenerator(),_mockLogger.Object);
                myAes.GenerateIV();
           
                var vector = myAes.IV;
            
                var encryptedText = aes.Encrypt(plainText, key, vector);
                var decryptedText = aes.Decrypt(encryptedText, key, vector);
                
                decryptedText.Should().Be(plainText);
            }  
        }

        [Fact]
        public void Encrypt_string_with_Invalid_Input_Should_Return_ArugumentNullException()
        {
            using (var myAes = Aes.Create())
            {
                var aes = new AesCryptoService(new Key256BitsGenerator(), _mockLogger.Object);

                Assert.ThrowsAny<ArgumentNullException>(() => aes.Encrypt(null, null));
            }
        }
    }
}
