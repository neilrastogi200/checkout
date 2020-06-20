using System;

namespace Payment.Gateway.Application.Encryption
{
    public class EncryptException : Exception
    {
        public EncryptException()
        {
            
        }
        
        public EncryptException(Exception innerException) : base("Encryption failed.", innerException)
        {
            
        }
    }
}