﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Application.HttpClient
{
    public class ApiClient : IApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<BankResponse> ProcessPayment(PaymentTransaction paymentTransaction)
        {
            if (paymentTransaction != null)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("payment");
                    var json = JsonConvert.SerializeObject(paymentTransaction);
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
                }

            }

            return null;
        }
    }

    public interface IApiClient
    {
        Task<BankResponse> ProcessPayment(PaymentTransaction paymentTransaction);
    }
}