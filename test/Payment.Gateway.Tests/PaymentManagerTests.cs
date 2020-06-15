using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
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
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
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
        public async Task HandlePayment_When_PaymentRequest_Is_Valid_Returns_ProcessPaymentTransactionResponse_Successfully()
        {
            //Arrange
            var cardData = TestDataBuilder.AddValidCardData();
            var currency = new Currency() {CurrencyId = 1, Name = "GBP"};

            var input = new PaymentRequest()
            {
                Amount = 300,
                Card = cardData,
                Currency = "GBP",
                MerchantId = "6662E78B-40E3-48AC-BBB5-21B97078B97A"
            };

            //var input = TestDataBuilder.AddValidPaymentRequest();

            var processPayment = new ProcessPayment()
            {
                Amount = input.Amount,
                CurrencyId = currency.CurrencyId,
                CardId = 1,
                MerchantId = input.MerchantId,
                Card = cardData,
            };

            //var processPayment = TestDataBuilder.AddValidProcessPayment();

            var expectedResult = new ProcessPaymentTransactionResponse()
            {
                Result = "Successfully processed and stored the payment transaction."
            };

            _mockCardDetailsService.Setup(x => x.IsValid(input.Card.CardExpiryMonth, input.Card.CardExpiryYear))
                .Returns(true);

            _mockCardDetailsService.Setup(x => x.AddCardDetails(input.Card)).Returns(1);

            _mockCurrencyRepository.Setup(x => x.GetCurrencyByName(input.Currency)).ReturnsAsync(currency);

            _mockMerchantRepository.Setup(x => x.GetMerchantById(new Guid(input.MerchantId)))
                .ReturnsAsync(new Merchant());

            _mockTransactionService.Setup(x => x.ProcessPaymentTransaction(It.Is<ProcessPayment>(y =>
                    y.Card == processPayment.Card && y.MerchantId == processPayment.MerchantId &&
                    y.CurrencyId == processPayment.CurrencyId && y.CardId == processPayment.CardId)))
                .ReturnsAsync(expectedResult);

            //_mockTransactionService.Setup(x => x.ProcessPaymentTransaction(processPayment))
            //    .ReturnsAsync(expectedResult);

            //Act
            var actualResult = await _paymentManager.HandlePayment(input);

            Assert.Equal(expectedResult,actualResult);
        }

        [Fact]
        public async Task HandlePayment_When_PaymentRequest_Has_Invalid_Expiry_Date_Returns_ProcessPaymentTransactionResponse_With_Errors()
        {
            //Arrange
            var cardData = new CardDetails()
            {
                CardExpiryYear = "2030",
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

            var expectedResult = new ProcessPaymentTransactionResponse()
            {
                Result = "Payment failed to process",
                ErrorMessage = new List<string>()
                {
                    new string("The card data is inValid. The expiryDate is wrong")
                }
            };

            _mockCardDetailsService.Setup(x => x.IsValid(input.Card.CardExpiryMonth, input.Card.CardExpiryYear))
                .Returns(false);

            //Act
            var actualResult = await _paymentManager.HandlePayment(input);

            Assert.Equal(expectedResult.ErrorMessage, actualResult.ErrorMessage);
            Assert.Equal(expectedResult.Result,actualResult.Result);
        }

        [Fact]
        public async Task HandlePayment_When_PaymentRequest_Has_No_Valid_Currency_And_MerchantId_Returns_ProcessPaymentTransactionResponse_With_Errors()
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

            var expectedResult = new ProcessPaymentTransactionResponse()
            {
                Result = "Payment failed to process",
                ErrorMessage = new List<string> { new string("The currency or merchantId is not supported. ") }
            };

            _mockCardDetailsService.Setup(x => x.IsValid(input.Card.CardExpiryMonth, input.Card.CardExpiryYear))
                .Returns(true);

            //Act
            var actualResult = await _paymentManager.HandlePayment(input);

            Assert.Equal(expectedResult.ErrorMessage, actualResult.ErrorMessage);
            Assert.Equal(expectedResult.Result, actualResult.Result);
        }
    }
}
