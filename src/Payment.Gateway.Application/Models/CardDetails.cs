namespace Payment.Gateway.Application.Models
{
    public class CardDetails
    {
        public string CardNumber { get; set; }
        
        public int CardExpiryMonth { get; set; }

        public int CardExpiryYear { get; set; }

        public string Cvv { get; set; }

        public string CardHolderName { get; set; }
    }
}
