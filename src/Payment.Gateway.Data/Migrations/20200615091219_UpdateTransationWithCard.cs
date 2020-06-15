using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.Gateway.Data.Migrations
{
    public partial class UpdateTransationWithCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("1685d492-e08e-484c-9f94-aee132e8d767"));

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("6662e78b-40e3-48ac-bbb5-21b97078b97a"));

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("e98baf34-3909-4a7c-8561-546268595af1"), true, "Test Merchant 1" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("218f9e24-c61f-4471-8a48-240d86da4a6a"), true, "Test Merchant 2" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_CardId",
                table: "PaymentTransactions",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_CardDetails_CardId",
                table: "PaymentTransactions",
                column: "CardId",
                principalTable: "CardDetails",
                principalColumn: "CardId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_CardDetails_CardId",
                table: "PaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTransactions_CardId",
                table: "PaymentTransactions");

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("218f9e24-c61f-4471-8a48-240d86da4a6a"));

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("e98baf34-3909-4a7c-8561-546268595af1"));

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("6662e78b-40e3-48ac-bbb5-21b97078b97a"), true, "Test Merchant 1" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("1685d492-e08e-484c-9f94-aee132e8d767"), true, "Test Merchant 2" });
        }
    }
}
