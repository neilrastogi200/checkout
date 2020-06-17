using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Payment.Gateway.Application.Exceptions;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Application.Services;
using Payment.Gateway.Data.Entities;
using Payment.Gateway.Data.Repositories;
using Payment_Gateway.Models;
using CardDetails = Payment.Gateway.Application.Models.CardDetails;

namespace Payment_Gateway
{
    public class PaymentManager : IPaymentManager
    {
        private readonly ICardDetailsService _cardDetailsService;
        private readonly ITransactionService _transactionService;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMerchantRepository _merchantRepository;
        private readonly ILogger<PaymentManager> _logger;

        public PaymentManager(ICardDetailsService cardDetailsService, ITransactionService transactionService,
            ICurrencyRepository currencyRepository, IMerchantRepository merchantRepository, ILogger<PaymentManager> logger)
        {
            _cardDetailsService = cardDetailsService ?? throw new ArgumentNullException(nameof(cardDetailsService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _currencyRepository = currencyRepository ?? throw  new ArgumentNullException(nameof(currencyRepository));
            _merchantRepository = merchantRepository ?? throw new ArgumentNullException(nameof(merchantRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<ProcessPaymentTransactionResponse> HandlePaymentAsync(PaymentRequest paymentRequest)
        {
            var isCardValid = _cardDetailsService.IsValid(paymentRequest.Card.CardExpiryMonth,paymentRequest.Card.CardExpiryYear);

            if (!isCardValid)
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

            _logger.LogInformation("The card details is valid");
            var currency = await _currencyRepository.GetCurrencyByNameAsync(paymentRequest.Currency);
            var merchant = await _merchantRepository.GetMerchantByIdAsync(new Guid(paymentRequest.MerchantId));

            if (currency != null && merchant != null)
            {
                _logger.LogInformation("Currency and Merchant are valid.");
                var cardId = _cardDetailsService.AddCardDetails(paymentRequest.Card);

                if (cardId > 0)
                {
                    var processPayment = new ProcessPayment
                    {
                        Amount = paymentRequest.Amount,
                        CurrencyId = currency.CurrencyId,
                        CardId = cardId,
                        MerchantId = paymentRequest.MerchantId,
                        Card = paymentRequest.Card
                    };

                    var paymentResult = await _transactionService.ProcessPaymentTransactionAsync(processPayment);
                    _logger.LogInformation("PaymentManager:HandlePayment:Process Payment has been processed.");
                    return paymentResult;
                }
            }
            else
            {
                var payment = new ProcessPaymentTransactionResponse
                {
                    Result = "Payment failed to process",
                    ErrorMessage = new List<string> {new string("The currency or merchantId is not supported. ")}
                };
                return payment;
            }

            _logger.LogError("There was problem adding your card data to the database.", LogLevel.Error);
            throw new DataApiException("There was problem adding your card data to the database.");
        }

      

        public async Task<PaymentTransactionResponse> GetPaymentTransactionByIdAsync(int paymentTransactionId)
        {
           var payment = await _transactionService.GetPaymentTransaction(paymentTransactionId);

           if (payment == null) return null;
           var currency = await _currencyRepository.GetCurrencyByIdAsync(payment.CurrencyId);
           var merchant = await _merchantRepository.GetMerchantByIdAsync(payment.MerchantId);
           var card = await _cardDetailsService.GetCardByIdAsync(payment.CardId);

           if (currency != null && merchant != null && card != null)
           {
               var paymentTransactionResponse = MapPaymentTransactionResponse(payment, currency, merchant, card);

               return paymentTransactionResponse;
           }
           return null;
        }

        private PaymentTransactionResponse MapPaymentTransactionResponse(PaymentTransaction payment, Currency currency,
            Merchant merchant, CardDetails card)
        {
            var paymentTransactionResponse = new PaymentTransactionResponse()
            {
                Amount = payment.Amount,
                Currency = currency.Name,
                MerchantName = merchant.Name,
                Card = new CardDetails()
                {
                    CardExpiryMonth = card.CardExpiryMonth,
                    CardExpiryYear = card.CardExpiryYear,
                    CardHolderName = card.CardHolderName,
                    CardNumber = _cardDetailsService.MaskCardNumber(card.CardNumber)
                },
                BankReferenceIdentifier = payment.BankIdentifier,
                Status = payment.Status
            };
            return paymentTransactionResponse;
        }
    }
}
