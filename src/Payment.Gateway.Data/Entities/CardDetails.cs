using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Gateway.Data.Entities
{
    public class CardDetails
    {
        [Key]
        public  int CardId { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiryMonth { get; set; }
        public string CardExpiryYear { get; set; }
        public string Cvv { get; set; }
        public string CardHolderName { get; set; }
    }
}
