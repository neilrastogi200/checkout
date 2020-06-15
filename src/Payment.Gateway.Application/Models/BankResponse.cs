using System;

namespace Payment.Gateway.Application.Models
{
    public class BankResponse
    {
        public Guid BankReferenceIdentifier { get; set; }

        public PaymentTransactionStatus Status { get; set; }
    }

    public enum PaymentTransactionStatus
    {
        Success,
        Failure
    }
}
