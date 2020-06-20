using System.ComponentModel.DataAnnotations;

namespace Payment.Gateway.Data.Entities
{
    public class CardDetails
    {
        [Key]
        public  int CardId { get; set; }
        public string CardNumber { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public string Cvv { get; set; }
        public string CardHolderName { get; set; }
    }
}
