using System;
using System.Collections.Generic;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Application.Models.Response;
using Payment.Gateway.Data.Entities;
using CardDetails = Payment.Gateway.Application.Models.CardDetails;

namespace Payment.Gateway.Tests
{
    public static class TestDataBuilder
    {
        public static CardDetails AddValidCardData()
        {
            return new CardDetails()
            {
                    CardExpiryYear = 2024,
                    CardExpiryMonth = 06,
                    CardHolderName = "Mr. Curtis",
                    Cvv = "100",
                    CardNumber = "4242424242424242"
            };
        }

        public static Data.Entities.CardDetails AddValidCardDataDto()
        {
            return new Data.Entities.CardDetails()
            {
                CardExpiryYear = 2024,
                CardExpiryMonth = 06,
                CardHolderName = "Mr. Curtis",
                Cvv = "100",
                CardNumber = "4242424242424242"
            };
        }
        public static CardDetails AddValidCardDataWithoutCvv()
        {
            return new CardDetails()
            {
                CardExpiryYear = 2024,
                CardExpiryMonth = 06,
                CardHolderName = "Mr. Curtis",
                CardNumber = "4242424242424242"
            };
        }

        public static CardDetails AddValidCardDataWithoutCvvAndMaskedCardNumber()
        {
            return new CardDetails()
            {
                CardExpiryYear = 2024,
                CardExpiryMonth = 06,
                CardHolderName = "Mr. Curtis",
                CardNumber = "XXXX XXXX XXXX 4242 "
            };
        }

        public static CardDetails AddInvalidExpiryDateCardData()
        {
            return new CardDetails()
            {
                CardExpiryYear = 2030,
                CardExpiryMonth = 06,
                CardHolderName = "Mr. Curtis",
                Cvv = "100",
                CardNumber = "4242424242424242"
            };
        }
        //ToDo: won't work resolve
        public static Application.Models.Payment AddValidPaymentRequest()
        {
            return new Application.Models.Payment()
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
                Card = AddValidCardData(),
            };
        }

        public static ProcessPaymentTransactionResponse AddExpectedResultSuccess()
        {
            return new ProcessPaymentTransactionResponse()
            {
                Result = "Successfully processed and stored the payment transaction."
            };
        }

        public static ProcessPaymentTransactionResponse AddInvalidExpiryDateFailure()
        {
            return new ProcessPaymentTransactionResponse()
            {
                Result = "Payment failed to process",
                ErrorMessage = new List<string>()
                {
                    new string("The card data is inValid. The expiryDate is wrong")
                }
            };
        }

        public static ProcessPaymentTransactionResponse AddCurrencyAndMerchantFailure()
        {
            return new ProcessPaymentTransactionResponse()
            {
                Result = "Payment failed to process",
                ErrorMessage = new List<string> { new string("The currency or merchantId is not supported. ") }
            };
        }

        public static Currency AddCurrency()
        {
            return new Currency()
            {
                CurrencyId = 1,
                Name = "GBP"
            };
        }
    }
}
