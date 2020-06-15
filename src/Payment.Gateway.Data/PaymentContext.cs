using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Merchant> Merchants { get; set; }

        public DbSet<CardDetails> CardDetails { get; set; }

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Currency>().ToTable("Currencies");
            builder.Entity<Merchant>().ToTable("Merchants");
            builder.Entity<PaymentTransaction>().ToTable("PaymentTransactions").Property(tr => tr.Amount).HasColumnType("decimal(18,2)");
            builder.Entity<CardDetails>().ToTable("CardDetails").Property(cd => cd.CardId).ValueGeneratedOnAdd();

            builder.Entity<Currency>().HasData(
                new Currency { CurrencyId = 1, Name = "GBP" },
                new Currency { CurrencyId = 2, Name = "USD" }
            );

            builder.Entity<Merchant>().HasData(
                new Merchant { MerchantId = Guid.NewGuid(), Name = "Test Merchant 1", IsActive = true},
                new Merchant { MerchantId = Guid.NewGuid(), Name = "Test Merchant 2", IsActive = true}
                
            );
        }
    }
}
