using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Gateway.Data.Entities
{
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }
        public string Name { get; set; }
    }
}
