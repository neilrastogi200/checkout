using System;
using Payment_Gateway.Models;

namespace Payment.Gateway.Application.Models
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