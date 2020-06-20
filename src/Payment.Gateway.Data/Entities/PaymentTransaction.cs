using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.Gateway.Data.Entities
{
    public class PaymentTransaction
    {
        [Key]
        public int PaymentTransactionId { get; set; }
        public decimal Amount { get; set; }

        [MaxLength(100)]
        public string Status { get; set; }
        public int CurrencyId { get; set; }
        public int CardId { get; set; }
        public Guid MerchantId { get; set; }
        public Guid BankIdentifier { get; set; }
        public Currency Currency { get; set; }
        public Merchant Merchant { get; set; }
        public CardDetails Card { get; set; }
    }
}
