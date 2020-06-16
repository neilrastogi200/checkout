using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
