using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Payment_Gateway;
using Xunit;

namespace Payment.Gateway.IntegrationTests
{
    public class AuthenticationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;

        public AuthenticationTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory ?? throw new ArgumentNullException(nameof(webApplicationFactory));
        }

        [Fact]
        public async Task When_Calling_The_GetPaymenttransaction_With_ApiKey_The_Ok_Response_Is_Returned()
        {
            //Arrange
            var httpClient = _webApplicationFactory.CreateClient();

            //ToDo : Ideally put in config file
            var apiKey = "072413ac-9b28-401f-a0d1-8512d7609dd8";
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

            var request = new HttpRequestMessage(HttpMethod.Get, "api/Payment/GetPaymentTransaction?id=14");

            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK,response.StatusCode); ;
        }


        [Fact]
        public async Task When_Calling_The_GetPaymenttransaction_Without_ApiKey_The_401_Is_Returned()
        {
            //Arrange
            var httpClient = _webApplicationFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "api/Payment/GetPaymentTransaction?id=6");

            //Act
            var response = await httpClient.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized,response.StatusCode); 
        }


    }
}
