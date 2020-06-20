using System.ComponentModel.DataAnnotations;

namespace Payment.Gateway.Data.Entities
{
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }
        public string Name { get; set; }
    }
}
