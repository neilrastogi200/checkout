using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Application.HttpClient
{
    public class ApiClient : IApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(IHttpClientFactory httpClientFactory, ILogger<ApiClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<BankResponse> ProcessPayment(ProcessPaymentData paymentData)
        {
            if (paymentData != null)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("payment");
                    var json = JsonConvert.SerializeObject(paymentData);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    using var response =
                        await client.PostAsync( "api/PaymentTransaction/ProcessPaymentTransaction",
                            data);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BankResponse>(content);
                    return result;
                }
                catch (AggregateException exceptions)
                {
                    exceptions.Handle(ex => true);
                   // _logger.LogError(exceptions);
                }

            }

            return null;
        }
    }

    public interface IApiClient
    {
        Task<BankResponse> ProcessPayment(ProcessPaymentData paymentData);
    }
}
