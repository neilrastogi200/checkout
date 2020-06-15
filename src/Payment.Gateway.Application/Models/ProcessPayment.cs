using Payment_Gateway.Models;

namespace Payment.Gateway.Application.Models
{
    public class ProcessPayment
    {
        public decimal Amount { get; set; }
        public CardDetails Card { get; set; }
        public int CardId { get; set; }
        public int CurrencyId { get; set; }
        public string MerchantId { get; set; }
    }
}
