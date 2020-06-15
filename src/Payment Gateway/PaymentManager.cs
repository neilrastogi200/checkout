using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Application.Services;
using Payment.Gateway.Data.Repositories;
using Payment_Gateway.Models;
using Payment_Gateway.Services;

namespace Payment_Gateway
{
    public class PaymentManager : IPaymentManager
    {
        private readonly ICardDetailsService _cardDetailsService;
        private readonly ITransactionService _transactionService;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMerchantRepository _merchantRepository;

        public PaymentManager(ICardDetailsService cardDetailsService, ITransactionService transactionService,
            ICurrencyRepository currencyRepository, IMerchantRepository merchantRepository)
        {
            _cardDetailsService = cardDetailsService ?? throw new ArgumentNullException(nameof(cardDetailsService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _currencyRepository = currencyRepository ?? throw  new ArgumentNullException(nameof(currencyRepository));
            _merchantRepository = merchantRepository ?? throw new ArgumentNullException(nameof(merchantRepository));
        }
        
        public async Task<ProcessPaymentTransactionResponse> HandlePayment(PaymentRequest paymentRequest)
        {
            //Check Validation of card details
            //Mapping
            var cardDetails = CardDetails(paymentRequest);
            var isCardValid = _cardDetailsService.IsValid(cardDetails.CardExpiryMonth,cardDetails.CardExpiryYear);

            if (isCardValid)
            {
               var currency = await _currencyRepository.GetCurrencyByName(paymentRequest.Currency);
               var merchant = await _merchantRepository.GetMerchantById(new Guid(paymentRequest.MerchantId));

               if (currency != null && merchant != null)
               {
                   var cardId = _cardDetailsService.AddCardDetails(cardDetails);

                   if (cardId > 0)
                   {
                       var processPayment = new ProcessPayment
                       {
                           Amount = paymentRequest.Amount,
                           CurrencyId = currency.CurrencyId,
                           CardId = cardId,
                           MerchantId = paymentRequest.MerchantId
                       };

                     var result = await _transactionService.ProcessPaymentTransaction(processPayment);
                   }

               }
               else
               {
                    var payment = new ProcessPaymentTransactionResponse
                    {
                        Result = "Payment failed to process",
                        ErrorMessage = new List<string> { new string("The currency or merchantId is not supported. ") }
                    };
                    return  payment;
               }
            }
            else
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

            return null;
        }

        private static CardDetails CardDetails(PaymentRequest paymentRequest)
        {
            var cardDetails = new CardDetails()
            {
                CardExpiryMonth = paymentRequest.Card.CardExpiryMonth,
                CardExpiryYear = paymentRequest.Card.CardExpiryYear,
                CardHolderName = paymentRequest.Card.CardHolderName,
                CardNumber = paymentRequest.Card.CardNumber,
                Cvv = paymentRequest.Card.Cvv
            };
            return cardDetails;
        }

        public async Task<PaymentTransactionResponse> GetPaymentTransactionById(int paymentTransactionId)
        {
           var payment = await _transactionService.GetPaymentTransaction(paymentTransactionId);

           var currency = await _currencyRepository.GetCurrencyById(payment.CurrencyId);

           var merchant = await _merchantRepository.GetMerchantById(payment.MerchantId);

           var card = await _cardDetailsService.GetCardById(payment.CardId);

           PaymentTransactionResponse paymentTransactionResponse = new PaymentTransactionResponse()
           {
               Amount = payment.Amount,
               Currency = currency.Name,
               MerchantName = merchant.Name,
               Card = new CardDetails()
               {
                   CardExpiryMonth = card.CardExpiryMonth,
                   CardExpiryYear = card.CardExpiryYear,
                   CardHolderName = card.CardHolderName,
                   CardNumber = _cardDetailsService.
                       MaskCardNumber(card.CardNumber)
               }

           };

           return paymentTransactionResponse;
        }
    }
}
