using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment_Gateway.Authentication
{
    public class InMemoryGetApiKey : IGetApiKey
    {
        private readonly IDictionary<string, ApiKey> _apiKeys;
        public InMemoryGetApiKey()
        {
            var existingApiKeys = new List<ApiKey>
            {
                new ApiKey("072413ac-9b28-401f-a0d1-8512d7609dd8", new DateTime(2020, 06, 16), 1)
            };
            
            _apiKeys = existingApiKeys.ToDictionary(x => x.Key, x => x);
        }

        public Task<ApiKey> Execute(string providedApiKey)
        {
            _apiKeys.TryGetValue(providedApiKey, out var key);
            return Task.FromResult(key);
        }
    }
}
