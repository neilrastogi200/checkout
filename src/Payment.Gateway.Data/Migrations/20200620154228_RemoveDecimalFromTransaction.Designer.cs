﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Payment.Gateway.Data;

namespace Payment.Gateway.Data.Migrations
{
    [DbContext(typeof(PaymentContext))]
    [Migration("20200620154228_RemoveDecimalFromTransaction")]
    partial class RemoveDecimalFromTransaction
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Payment.Gateway.Data.Entities.CardDetails", b =>
                {
                    b.Property<int>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CardExpiryMonth")
                        .HasColumnType("int");

                    b.Property<int>("CardExpiryYear")
                        .HasColumnType("int");

                    b.Property<string>("CardHolderName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cvv")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CardId");

                    b.ToTable("CardDetails");
                });

            modelBuilder.Entity("Payment.Gateway.Data.Entities.Currency", b =>
                {
                    b.Property<int>("CurrencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CurrencyId");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            CurrencyId = 1,
                            Name = "GBP"
                        },
                        new
                        {
                            CurrencyId = 2,
                            Name = "USD"
                        });
                });

            modelBuilder.Entity("Payment.Gateway.Data.Entities.Merchant", b =>
                {
                    b.Property<Guid>("MerchantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MerchantId");

                    b.ToTable("Merchants");

                    b.HasData(
                        new
                        {
                            MerchantId = new Guid("77d0ea5d-32a8-45c3-847a-a00541cdf1e3"),
                            IsActive = true,
                            Name = "Test Merchant 1"
                        },
                        new
                        {
                            MerchantId = new Guid("510c2a60-89bc-43a1-aa32-d3eaf2408d8a"),
                            IsActive = true,
                            Name = "Test Merchant 2"
                        });
                });

            modelBuilder.Entity("Payment.Gateway.Data.Entities.PaymentTransaction", b =>
                {
                    b.Property<int>("PaymentTransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("BankIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("PaymentTransactionId");

                    b.HasIndex("CardId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("MerchantId");

                    b.ToTable("PaymentTransactions");
                });

            modelBuilder.Entity("Payment.Gateway.Data.Entities.PaymentTransaction", b =>
                {
                    b.HasOne("Payment.Gateway.Data.Entities.CardDetails", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Payment.Gateway.Data.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Payment.Gateway.Data.Entities.Merchant", "Merchant")
                        .WithMany()
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
