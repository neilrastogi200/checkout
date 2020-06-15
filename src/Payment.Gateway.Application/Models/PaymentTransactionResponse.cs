using System;

namespace Payment_Gateway.Models
{
    public class PaymentTransactionResponse
    {
        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public CardDetails Card { get; set; }

        public string MerchantName { get; set; }

        public Guid BankReferenceIdentifier { get; set; }
    }
}