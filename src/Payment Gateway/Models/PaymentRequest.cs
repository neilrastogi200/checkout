﻿using System.ComponentModel.DataAnnotations;

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
