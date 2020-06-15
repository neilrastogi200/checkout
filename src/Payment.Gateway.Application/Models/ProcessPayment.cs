namespace Payment.Gateway.Application.Models
{
    public class ProcessPayment
    {
        public string MerchantId { get; set; }
        public decimal Amount { get; set; }
        public int CardId { get; set; }
        public int CurrencyId { get; set; }
    }
}
