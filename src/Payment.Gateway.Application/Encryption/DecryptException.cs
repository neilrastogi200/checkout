using System;

namespace Payment.Gateway.Application.Encryption
{
    public class DecryptException : Exception
    {
        public DecryptException()
        {
            
        }
        
        public DecryptException(Exception innerException) : base("Decryption failed.", innerException)
        {
            
        }
    }
}