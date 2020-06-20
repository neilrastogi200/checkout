namespace Payment.Gateway.Application.Encryption
{
    public interface IAesCryptoService
    {   
        string Encrypt(string text, string key, byte[] IV = null);
        
        string Decrypt(string cipherText, string key, byte[] IV = null);
    }
}