using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Gateway.Data.Entities
{
    public class Merchant
    {
        [Key]
        public Guid MerchantId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
