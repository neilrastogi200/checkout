﻿using System;
using System.Threading.Tasks;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data.Repositories
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly PaymentContext _paymentContext;
        public MerchantRepository(PaymentContext paymentContext)
        {
            _paymentContext = paymentContext;
        }

        public async Task<Merchant> GetMerchantByIdAsync(Guid merchantId)
        {
            return await _paymentContext.Merchants.FindAsync(merchantId);
        }
    }
}
