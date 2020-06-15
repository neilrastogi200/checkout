using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Gateway.MockBank.Models
{
    public class MockBankPaymentRequest
    {
        public decimal TransactionAmount { get; set; }
        public string CardNumber { get; set; }
        public string CardCvv { get; set; }
        public string CardHolderName { get; set; }
        public string CardExpiryMonth { get; set; }
        public string CardExpiryYear { get; set; }
    }
}
