using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Gateway.Application.Models
{
    public class ProcessPaymentData
    {
        public CardDetails Card { get; set; }
        public decimal Amount { get; set; }
    }
}
