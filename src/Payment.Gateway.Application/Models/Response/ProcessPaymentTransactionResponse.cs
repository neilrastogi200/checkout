using System.Collections.Generic;

namespace Payment.Gateway.Application.Models.Response
{
    public class ProcessPaymentTransactionResponse
    {
        public string Result { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
}
