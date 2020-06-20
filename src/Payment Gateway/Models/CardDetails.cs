using System.ComponentModel.DataAnnotations;

namespace Payment_Gateway.Models
{
    public class CardDetails
    {
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression("^(0[1-9]|1[0-2])$")]
        public string CardExpiryMonth { get; set; }
        [Required]
        [RegularExpression("^20[0-9]{2}$")]
        public string CardExpiryYear { get; set; }

        [Required]
        [RegularExpression("^\\d{3}$")]
        public string Cvv { get; set; }

        [Required]
        public string CardHolderName { get; set; }
    }
}
