using System;
using Microsoft.Extensions.Logging;
using Moq;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Application.Services;
using Payment.Gateway.Data.Entities;
using Payment.Gateway.Data.Repositories;
using Payment_Gateway;
using Payment_Gateway.Models;
using Xunit;
using CardDetails = Payment.Gateway.Application.Models.CardDetails;

namespace Payment.Gateway.Tests
{
    public class PaymentManagerTests
    {
        private readonly Mock<ICardDetailsService> _mockCardDetailsService;
        private readonly Mock<ITransactionService> _mockTransactionService;
        private readonly Mock<ICardDetailsRepository> _mockCardDetailsRepository;
        private readonly Mock<ICurrencyRepository> _mockCurrencyRepository;
        private readonly Mock<IMerchantRepository> _mockMerchantRepository;
        private readonly Mock<ILogger<PaymentManager>> _mockLogger;
        private readonly IPaymentManager _paymentManager;
        public PaymentManagerTests()
        {
            _mockCardDetailsRepository = new Mock<ICardDetailsRepository>();
            _mockCardDetailsService = new Mock<ICardDetailsService>();
            _mockMerchantRepository = new Mock<IMerchantRepository>();
            _mockTransactionService = new Mock<ITransactionService>();
            _mockCurrencyRepository = new Mock<ICurrencyRepository>();
            _mockLogger = new Mock<ILogger<PaymentManager>>();

            _paymentManager = new PaymentManager(_mockCardDetailsService.Object,_mockTransactionService.Object,_mockCurrencyRepository.Object,_mockMerchantRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void HandlePayment_When_PaymentRequest_Is_Valid_Returns_ProcessPaymentTransactionResponse_Successfully()
        {
            //Arrange

            var cardData = new CardDetails()
            {
                CardExpiryYear = "2024",
                CardExpiryMonth = "06",
                CardHolderName = "Mr. Curtis",
                Cvv = "100",
                CardNumber = "4242424242424242"
            };

            var input = new PaymentRequest()
            {
                Amount = 300,
                Card = cardData,
                Currency = "GBP",
                MerchantId = "6662E78B-40E3-48AC-BBB5-21B97078B97A"
            };

           

            var processPayment = new ProcessPayment()
            {
                Amount = input.Amount,
                Card = cardData,
                CardId = 1,
                CurrencyId = 1,
                MerchantId = input.MerchantId
            };

            var expectedResult = new ProcessPaymentTransactionResponse()
            {
                Result = "Successfully processed and stored the payment transaction."
            };

            _mockCardDetailsService.Setup(x => x.IsValid(input.Card.CardExpiryMonth, input.Card.CardExpiryYear))
                .Returns(true);

            _mockCardDetailsService.Setup(x => x.AddCardDetails(input.Card)).Returns(1);

            _mockCurrencyRepository.Setup(x => x.GetCurrencyByName(input.Currency)).ReturnsAsync(new Currency());

            _mockMerchantRepository.Setup(x => x.GetMerchantById(new Guid(input.MerchantId)))
                .ReturnsAsync(new Merchant());

            _mockTransactionService.Setup(x => x.ProcessPaymentTransaction(It.IsAny<ProcessPayment>()))
                .ReturnsAsync(expectedResult);

            //Act
           var actualResult = _paymentManager.HandlePayment(input);
        }
    }
}
