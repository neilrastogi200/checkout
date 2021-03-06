﻿using System;
using System.Threading.Tasks;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data.Repositories
{
    public interface IMerchantRepository
    {
        Task<Merchant> GetMerchantByIdAsync(Guid merchantId);

    }
}
