using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var httpClient = _webApplicationFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("X-Api-Key", "072413ac-9b28-401f-a0d1-8512d7609dd8");
            
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Payment/GetPaymentTransaction?id=1");

            var response = await httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            //Assert.Equal(200,); ;
        }

    }
}
