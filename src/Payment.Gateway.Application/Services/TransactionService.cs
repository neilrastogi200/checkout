using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Payment.Gateway.Application.Exceptions;
using Payment.Gateway.Application.HttpClient;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Application.Models.Response;
using Payment.Gateway.Data.Entities;
using Payment.Gateway.Data.Repositories;
using CardDetails = Payment.Gateway.Application.Models.CardDetails;

namespace Payment.Gateway.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IApiClient _apiClient;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ITransactionRepository transactionRepository, IApiClient apiClient, ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProcessPaymentTransactionResponse> ProcessPaymentTransactionAsync(ProcessPayment payment)
        {
            var paymentTransaction = CreatePaymentTransaction(payment);
            var transactionAdded = _transactionRepository.AddPaymentTransaction(paymentTransaction);

            if (transactionAdded > 0)
            {
                var paymentData = MapProcessPaymentData(payment);
                var bankResponse = await _apiClient.ProcessPayment(paymentData);
                UpdateTransaction(paymentTransaction, bankResponse);

                if (Enum.GetName(typeof(PaymentTransactionStatus), bankResponse.Status) == "Success")
                  return new ProcessPaymentTransactionResponse
                  {
                      Result = paymentTransaction.Status + " " + bankResponse.Message
                  };
                return new ProcessPaymentTransactionResponse
                {
                    Result = paymentTransaction.Status,
                    ErrorMessage = new List<string>
                        {Enum.GetName(typeof(PaymentTransactionSubStatus), bankResponse.Message)}
                };
            }

            _logger.LogError("The transaction has failed to be added", LogLevel.Error);
            throw new DataApiException("The transaction has failed to be added");
        }

        private void UpdateTransaction(PaymentTransaction paymentTransaction, BankResponse bankResponse)
        {
            paymentTransaction.Status = Enum.GetName(typeof(PaymentTransactionStatus), bankResponse.Status);
            paymentTransaction.BankIdentifier = bankResponse.BankReferenceIdentifier;

            _transactionRepository.UpdateTransaction(paymentTransaction);
        }

        private static ProcessPaymentData MapProcessPaymentData(ProcessPayment payment)
        {
            var paymentData = new ProcessPaymentData()
            {
                Card = new CardDetails()
                {
                    CardExpiryYear = payment.Card.CardExpiryYear,
                    CardExpiryMonth = payment.Card.CardExpiryMonth,
                    Cvv = payment.Card.Cvv,
                    CardHolderName = payment.Card.CardHolderName,
                    CardNumber = payment.Card.CardNumber
                },
                Amount = payment.Amount
            };
            return paymentData;
        }

        private PaymentTransaction CreatePaymentTransaction(ProcessPayment payment)
        {
            var paymentTransaction =
                new PaymentTransaction()
                {
                    MerchantId = new Guid(payment.MerchantId),
                    Amount = payment.Amount,
                    CardId = payment.CardId,
                    CurrencyId = payment.CurrencyId,
                };
            return paymentTransaction;
        }

        public async Task<PaymentTransaction> GetPaymentTransaction(int paymentTransactionId)
        {
           var paymentTransaction = await _transactionRepository.GetPaymentTransactionAsync(paymentTransactionId);

           return paymentTransaction;
        }
    }
}
