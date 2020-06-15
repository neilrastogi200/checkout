using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Payment.Gateway.Data.Entities
{
    public class PaymentTransaction
    {
        [Key]
        public int PaymentTransactionId { get; set; }
        [DataType("decimal(18,5)")]
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
