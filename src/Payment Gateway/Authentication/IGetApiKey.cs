using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment_Gateway.Authentication
{
    public interface IGetApiKey
    {
        Task<ApiKey> Execute(string providedApiKey);
    }
}
