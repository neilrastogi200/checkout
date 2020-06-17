using System.Threading.Tasks;
using Payment.Gateway.Application.Models;

namespace Payment.Gateway.Application.HttpClient
{
    public interface IApiClient
    {
        Task<BankResponse> ProcessPayment(ProcessPaymentData paymentData);
    }
}