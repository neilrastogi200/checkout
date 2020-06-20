using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.Gateway.Data.Migrations
{
    public partial class RemoveDecimalFromTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("4021c9af-1dc9-4292-b2cc-7fa8119b6797"));

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("87c4da44-94d4-4ddc-b242-20a5098fe614"));

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("77d0ea5d-32a8-45c3-847a-a00541cdf1e3"), true, "Test Merchant 1" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("510c2a60-89bc-43a1-aa32-d3eaf2408d8a"), true, "Test Merchant 2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("510c2a60-89bc-43a1-aa32-d3eaf2408d8a"));

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("77d0ea5d-32a8-45c3-847a-a00541cdf1e3"));

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("4021c9af-1dc9-4292-b2cc-7fa8119b6797"), true, "Test Merchant 1" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("87c4da44-94d4-4ddc-b242-20a5098fe614"), true, "Test Merchant 2" });
        }
    }
}
