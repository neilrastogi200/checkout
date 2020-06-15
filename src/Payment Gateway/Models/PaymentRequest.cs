using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Payment.Gateway.Application.Models;

namespace Payment_Gateway.Models
{
    public class PaymentRequest
    {
        [Required]
        public string MerchantId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public CardDetails Card { get; set; }
    }
}
