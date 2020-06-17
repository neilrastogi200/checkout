using System;

namespace Payment_Gateway.Authentication
{
    public class ApiKey
    {
        public string Key { get; }
        public DateTime Created { get; }
        public int Id { get; }

        public ApiKey(string key, DateTime created, int id)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Id = id;
            Created = created;
        }

       
    }
}
