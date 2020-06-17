using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payment.Gateway.Application.Models;

namespace Payment.Gateway.MockBank.Models
{
    public class MockBankPaymentRequest
    {
        public decimal Amount { get; set; }
        public CardDetails Card { get; set; }
    }
}
