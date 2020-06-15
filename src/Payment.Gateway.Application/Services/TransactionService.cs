using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payment.Gateway.Application.HttpClient;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Application.Services;
using Payment.Gateway.Data.Entities;
using Payment.Gateway.Data.Repositories;
using Payment_Gateway.Models;

namespace Payment_Gateway.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IApiClient _apiClient;

        public TransactionService(ITransactionRepository transactionRepository, IApiClient apiClient)
        {
            _transactionRepository = transactionRepository;
            _apiClient = apiClient;
        }

        public Task<PaymentTransactionResponse> ProcessPaymentTransaction(ProcessPayment payment)
        {
            var paymentTransaction = CreatePaymentTransaction(payment);

            _transactionRepository.AddPaymentTransaction(paymentTransaction);

            //Call the bank httpClient
            _apiClient.ProcessPayment(paymentTransaction);

            return null;
        }

        private static PaymentTransaction CreatePaymentTransaction(ProcessPayment payment)
        {
            PaymentTransaction paymentTransaction =
                new PaymentTransaction()
                {
                    MerchantId = new Guid(payment.MerchantId),
                    Amount = payment.Amount,
                    CardId = payment.CardId,
                    CurrencyId = payment.CurrencyId,
                    Status = "Failed"
                };
            return paymentTransaction;
        }

        public async Task<PaymentTransaction> GetPaymentTransaction(int paymentTransactionId)
        {
           var paymentTransaction = await _transactionRepository.GetPaymentTransaction(paymentTransactionId);

           return paymentTransaction;
        }
    }
}
