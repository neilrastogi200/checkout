using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.Gateway.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "CardDetails",
                columns: table => new
                {
                    CardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(nullable: true),
                    CardExpiryMonth = table.Column<string>(nullable: true),
                    CardExpiryYear = table.Column<string>(nullable: true),
                    Cvv = table.Column<string>(nullable: true),
                    CardHolderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDetails", x => x.CardId);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    IsoCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    MerchantId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.MerchantId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    PaymentTransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(maxLength: 100, nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    CardId = table.Column<int>(nullable: false),
                    MerchantId = table.Column<Guid>(nullable: false),
                    BankIdentifier = table.Column<Guid>(nullable: false),
                    BankId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.PaymentTransactionId);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Currencies",
                columns: new[] { "CurrencyId", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 1, null, "GBP" },
                    { 2, null, "USD" }
                });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("7080d0ef-59a5-4088-915a-6bad7ac426fc"), true, "Test Merchant 1" },
                    { new Guid("8902770c-b8e6-4c5b-b6b5-69ed2cfb6938"), true, "Test Merchant 2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_BankId",
                table: "PaymentTransactions",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_CurrencyId",
                table: "PaymentTransactions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_MerchantId",
                table: "PaymentTransactions",
                column: "MerchantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardDetails");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Merchants");
        }
    }
}
