using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.Gateway.Data.Migrations
{
    public partial class RemoveBank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Banks_BankId",
                table: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTransactions_BankId",
                table: "PaymentTransactions");

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("74fe445f-aead-4aab-a63c-6215d85993a5"));

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("ff993e54-52b8-4ec2-843c-bfb805f6e3b9"));

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "PaymentTransactions");

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("6662e78b-40e3-48ac-bbb5-21b97078b97a"), true, "Test Merchant 1" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[] { new Guid("1685d492-e08e-484c-9f94-aee132e8d767"), true, "Test Merchant 2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("1685d492-e08e-484c-9f94-aee132e8d767"));

            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: new Guid("6662e78b-40e3-48ac-bbb5-21b97078b97a"));

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "PaymentTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "BankId", "BankName" },
                values: new object[,]
                {
                    { 1, "TestBank" },
                    { 2, "TestBank2" }
                });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("ff993e54-52b8-4ec2-843c-bfb805f6e3b9"), true, "Test Merchant 1" },
                    { new Guid("74fe445f-aead-4aab-a63c-6215d85993a5"), true, "Test Merchant 2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_BankId",
                table: "PaymentTransactions",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_Banks_BankId",
                table: "PaymentTransactions",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "BankId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
