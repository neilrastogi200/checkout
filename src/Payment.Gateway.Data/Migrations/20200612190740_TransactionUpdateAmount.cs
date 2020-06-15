using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.Gateway.Data.Migrations
{
    public partial class TransactionUpdateAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("1b436886-9eec-4fb2-a72d-62887b469295"));

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("1d9a0bd1-6504-4761-81d3-34ff034350f9"));

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("f5a96033-9694-4e71-a9f1-15e0e8b83a2e"), true, "Test Merchant 1" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("9892b287-ac96-4b2d-b94b-ff902ad4d3e4"), true, "Test Merchant 2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("9892b287-ac96-4b2d-b94b-ff902ad4d3e4"));

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("f5a96033-9694-4e71-a9f1-15e0e8b83a2e"));

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("1b436886-9eec-4fb2-a72d-62887b469295"), true, "Test Merchant 1" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("1d9a0bd1-6504-4761-81d3-34ff034350f9"), true, "Test Merchant 2" });
        }
    }
}
