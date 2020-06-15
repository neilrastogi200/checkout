using System;
using System.Collections.Generic;
using System.Text;
using Payment.Gateway.Application.Models;
using Payment_Gateway.Models;

namespace Payment.Gateway.Tests
{
    public static class TestDataBuilder
    {
        private static CardDetails CardDetails => AddValidCardData();

        public static CardDetails AddValidCardData()
        {
            return new CardDetails()
            {
                    CardExpiryYear = "2024",
                    CardExpiryMonth = "06",
                    CardHolderName = "Mr. Curtis",
                    Cvv = "100",
                    CardNumber = "4242424242424242"
            };
        }

        public static PaymentRequest AddValidPaymentRequest()
        {
            return new PaymentRequest()
            {
                Amount = 300,
                Card = AddValidCardData(),
                Currency = "GBP",
                MerchantId = "6662E78B-40E3-48AC-BBB5-21B97078B97A"
            };
        }

        public static ProcessPayment AddValidProcessPayment()
        {
            return new ProcessPayment()
            {
                Amount = AddValidPaymentRequest().Amount,
                CurrencyId = 1,
                CardId = 1,
                MerchantId = AddValidPaymentRequest().MerchantId,
                Card = CardDetails,
            };
        }
    }
}
