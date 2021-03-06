using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Payment.Gateway.Application;
using Payment.Gateway.Application.Exceptions;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Application.Models.Response;
using Payment.Gateway.Application.Services;
using Payment.Gateway.Data.Entities;
using Payment.Gateway.Data.Repositories;
using Xunit;

namespace Payment.Gateway.Tests
{
    public class PaymentManagerTests
    {
        private readonly Mock<ICardDetailsService> _mockCardDetailsService;
        private readonly Mock<ITransactionService> _mockTransactionService;
        private readonly Mock<ICurrencyRepository> _mockCurrencyRepository;
        private readonly Mock<IMerchantRepository> _mockMerchantRepository;
        private readonly Mock<ILogger<PaymentManager>> _mockLogger;
        private readonly IPaymentManager _paymentManager;
        public PaymentManagerTests()
        {
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

            var input = new Application.Models.Payment()
            {
                Amount = 300,
                Card = cardData,
                Currency = "GBP",
                MerchantId = "6662E78B-40E3-48AC-BBB5-21B97078B97A"
            };

            var processPayment = new ProcessPayment()
            {
                Amount = input.Amount,
                CurrencyId = currency.CurrencyId,
                CardId = 1,
                MerchantId = input.MerchantId,
                Card = cardData,
            };

            var expectedResult = TestDataBuilder.AddExpectedResultSuccess();

            _mockCardDetailsService.Setup(x => x.IsValid(input.Card.CardExpiryMonth, input.Card.CardExpiryYear))
                .Returns(true);

            _mockCardDetailsService.Setup(x => x.AddCardDetails(input.Card)).Returns(1);

            _mockCurrencyRepository.Setup(x => x.GetCurrencyByNameAsync(input.Currency)).ReturnsAsync(currency);

            _mockMerchantRepository.Setup(x => x.GetMerchantByIdAsync(new Guid(input.MerchantId)))
                .ReturnsAsync(new Merchant());

            _mockTransactionService.Setup(x => x.ProcessPaymentTransactionAsync(It.Is<ProcessPayment>(y =>
                    y.Card == processPayment.Card && y.MerchantId == processPayment.MerchantId &&
                    y.CurrencyId == processPayment.CurrencyId && y.CardId == processPayment.CardId)))
                .ReturnsAsync(expectedResult);

            //Act
            var actualResult = await _paymentManager.HandlePaymentAsync(input);

            Assert.Equal(expectedResult,actualResult);
        }

        [Fact]
        public async Task HandlePayment_When_PaymentRequest_Has_Invalid_Expiry_Date_Returns_ProcessPaymentTransactionResponse_With_Errors()
        {
            //Arrange
            var cardData = TestDataBuilder.AddInvalidExpiryDateCardData();

            var input = new Application.Models.Payment()
            {
                Amount = 300,
                Card = cardData,
                Currency = "GBP",
                MerchantId = "6662E78B-40E3-48AC-BBB5-21B97078B97A"
            };

            var expectedResult = TestDataBuilder.AddInvalidExpiryDateFailure();

            _mockCardDetailsService.Setup(x => x.IsValid(input.Card.CardExpiryMonth, input.Card.CardExpiryYear))
                .Returns(false);

            //Act
            var actualResult = await _paymentManager.HandlePaymentAsync(input);

            Assert.Equal(expectedResult.ErrorMessage, actualResult.ErrorMessage);
            Assert.Equal(expectedResult.Result,actualResult.Result);
        }

        [Fact]
        public async Task HandlePayment_When_PaymentRequest_Has_No_Valid_Currency_And_MerchantId_Returns_ProcessPaymentTransactionResponse_With_Errors()
        {
            //Arrange
            var cardData = TestDataBuilder.AddValidCardData();

            var input = new Application.Models.Payment()
            {
                Amount = 300,
                Card = cardData,
                Currency = "FRC",
                MerchantId = "6662E78B-40E3-48AC-BBB5-21B97078B97A"
            };

            var expectedResult = TestDataBuilder.AddCurrencyAndMerchantFailure();

            _mockCardDetailsService.Setup(x => x.IsValid(input.Card.CardExpiryMonth, input.Card.CardExpiryYear))
                .Returns(true);
            
            //Act
            var actualResult = await _paymentManager.HandlePaymentAsync(input);

            Assert.Equal(expectedResult.ErrorMessage, actualResult.ErrorMessage);
            Assert.Equal(expectedResult.Result, actualResult.Result);
        }

        [Fact]
        public async Task HandlePayment_When_PaymentRequest_Is_Valid_And_CardData_Is_Not_Added_To_Database_Returns_ProcessPaymentTransactionResponse_With_Errors()
        {
            //Arrange
            var cardData = TestDataBuilder.AddValidCardData();

            var input = new Application.Models.Payment()
            {
                Amount = 300,
                Card = cardData,
                Currency = "GBP",
                MerchantId = "6662E78B-40E3-48AC-BBB5-21B97078B97A"
            };

            _mockCardDetailsService.Setup(x => x.IsValid(input.Card.CardExpiryMonth, input.Card.CardExpiryYear))
                .Returns(true);

            _mockCurrencyRepository.Setup(x => x.GetCurrencyByNameAsync(input.Currency)).ReturnsAsync(new Currency());

            _mockMerchantRepository.Setup(x => x.GetMerchantByIdAsync(new Guid(input.MerchantId)))
                .ReturnsAsync(new Merchant());

            //Act/Assert
            await Assert.ThrowsAsync<DataApiException>(async () => await _paymentManager.HandlePaymentAsync(input));
        }

        [Fact]
        public async Task GetPaymentTransactionById_When_Input_Is_Valid_Returns_PaymentTransactionResponse()
        {
            //Arrange
            var input = 1;
            var merchantIdentifier = Guid.NewGuid();
            var bankId = Guid.NewGuid();
            var maskedCardNumber = "XXXX XXXX XXXX 4242 ";

            var currency = TestDataBuilder.AddCurrency();
            
            var merchant = new Merchant()
            {
                IsActive = true,
                MerchantId = merchantIdentifier,
                Name = "Test1"
            };

            var cardData = TestDataBuilder.AddValidCardDataDto();

            var cardDataApplication = TestDataBuilder.AddValidCardDataWithoutCvv();

            var payment = new PaymentTransaction()
            {
                Amount = 140,
                BankIdentifier = bankId,
                Card = cardData,
                CardId = 1,
                Currency = currency,
                CurrencyId = 1,
                Merchant = merchant,
                MerchantId = merchantIdentifier,
                Status = "Success",
                PaymentTransactionId = 1,
            };

            var expectedResult = new PaymentTransactionResponse()
            {
                Amount = payment.Amount,
                BankReferenceIdentifier = payment.BankIdentifier,
                Card = TestDataBuilder.AddValidCardDataWithoutCvvAndMaskedCardNumber(),
                Status = payment.Status,
                Currency = payment.Currency.Name,
                MerchantName = payment.Merchant.Name
            };

            _mockTransactionService.Setup(x => x.GetPaymentTransaction(input)).ReturnsAsync(payment);
            _mockCurrencyRepository.Setup(x => x.GetCurrencyByIdAsync(currency.CurrencyId)).ReturnsAsync(currency);
            _mockMerchantRepository.Setup(x => x.GetMerchantByIdAsync(merchantIdentifier)).ReturnsAsync(merchant);
            _mockCardDetailsService.Setup(x => x.GetCardByIdAsync(payment.CardId)).ReturnsAsync(cardDataApplication);
            _mockCardDetailsService.Setup(x => x.MaskCardNumber(payment.Card.CardNumber)).Returns(maskedCardNumber);

            //Act
           var actualResult = await _paymentManager.GetPaymentTransactionByIdAsync(input);

           //Assert
            Assert.Equal(expectedResult.Card.CardExpiryMonth,actualResult.Card.CardExpiryMonth);
            Assert.Equal(expectedResult.Card.CardExpiryYear, actualResult.Card.CardExpiryYear);
            Assert.Equal(expectedResult.Card.CardHolderName, actualResult.Card.CardHolderName);
            Assert.Equal(expectedResult.Card.CardNumber, actualResult.Card.CardNumber);
            Assert.Equal(expectedResult.Amount, actualResult.Amount);
            Assert.Equal(expectedResult.Status, actualResult.Status);
            Assert.Equal(expectedResult.Currency, actualResult.Currency);
            Assert.Equal(expectedResult.BankReferenceIdentifier, actualResult.BankReferenceIdentifier);
            Assert.Equal(expectedResult.MerchantName, actualResult.MerchantName);
        }

        [Fact]
        public async void GetPaymentTransactionId_When_TransactionId_Does_Not_Exist_Returns_Null()
        {
            //Arrange
            var input = 8;

            //Act
            var actualResult = await _paymentManager.GetPaymentTransactionByIdAsync(input);

            //Assert
            Assert.Null(actualResult);
        }
    }
}
