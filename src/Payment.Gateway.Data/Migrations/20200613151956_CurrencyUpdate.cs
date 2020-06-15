using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.Gateway.Data.Migrations
{
    public partial class CurrencyUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("9892b287-ac96-4b2d-b94b-ff902ad4d3e4"));

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("f5a96033-9694-4e71-a9f1-15e0e8b83a2e"));

            migrationBuilder.DropColumn(
                name: "IsoCode",
                table: "Currencies");

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("ff993e54-52b8-4ec2-843c-bfb805f6e3b9"), true, "Test Merchant 1" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("74fe445f-aead-4aab-a63c-6215d85993a5"), true, "Test Merchant 2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("74fe445f-aead-4aab-a63c-6215d85993a5"));

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("ff993e54-52b8-4ec2-843c-bfb805f6e3b9"));

            migrationBuilder.AddColumn<string>(
                name: "IsoCode",
                table: "Currencies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("f5a96033-9694-4e71-a9f1-15e0e8b83a2e"), true, "Test Merchant 1" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("9892b287-ac96-4b2d-b94b-ff902ad4d3e4"), true, "Test Merchant 2" });
        }
    }
}
