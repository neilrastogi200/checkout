using System;
using System.Threading.Tasks;
using Payment.Gateway.Application.HttpClient;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Data.Entities;
using Payment.Gateway.Data.Repositories;
using Payment_Gateway.Models;
using CardDetails = Payment_Gateway.Models.CardDetails;

namespace Payment.Gateway.Application.Services
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

        public async Task<ProcessPaymentTransactionResponse> ProcessPaymentTransaction(ProcessPayment payment)
        {
            ProcessPaymentData paymentData = new ProcessPaymentData()
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

            //Call the bank httpClient
           var bankResponse = await _apiClient.ProcessPayment(paymentData);

           var paymentTransaction = CreatePaymentTransaction(payment, bankResponse);

          var transactionAdded = _transactionRepository.AddPaymentTransaction(paymentTransaction);

          if (transactionAdded > 0)
          {
            return new ProcessPaymentTransactionResponse()
            {
                Result = "Successfully processed and stored the payment transaction."
            };
          }
            return null;
        }

        private static PaymentTransaction CreatePaymentTransaction(ProcessPayment payment, BankResponse bankResponse)
        {
            PaymentTransaction paymentTransaction =
                new PaymentTransaction()
                {
                    MerchantId = new Guid(payment.MerchantId),
                    Amount = payment.Amount,
                    CardId = payment.CardId,
                    CurrencyId = payment.CurrencyId,
                    Status = Enum.GetName(typeof(PaymentTransactionStatus),bankResponse.Status),
                    BankIdentifier = bankResponse.BankReferenceIdentifier
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
