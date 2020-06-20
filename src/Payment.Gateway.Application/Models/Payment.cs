namespace Payment.Gateway.Application.Models
{
    public class Payment
    {
        public string MerchantId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public CardDetails Card { get; set; }
    }
}
