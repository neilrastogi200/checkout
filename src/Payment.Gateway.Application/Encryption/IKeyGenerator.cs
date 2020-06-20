namespace Payment.Gateway.Application.Encryption
{
    public interface IKeyGenerator
    {
        byte[] Generate(string input);
        
        int GetKeySize();
    }
}