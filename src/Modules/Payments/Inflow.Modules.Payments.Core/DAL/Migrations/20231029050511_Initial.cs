﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inflow.Modules.Payments.Core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "payments");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Nationality = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositAccounts",
                schema: "payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Iban = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositAccounts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "payments",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawalAccounts",
                schema: "payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Iban = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawalAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WithdrawalAccounts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "payments",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deposits",
                schema: "payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deposits_DepositAccounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "payments",
                        principalTable: "DepositAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Withdrawals",
                schema: "payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Withdrawals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Withdrawals_WithdrawalAccounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "payments",
                        principalTable: "WithdrawalAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccounts_CustomerId_Currency",
                schema: "payments",
                table: "DepositAccounts",
                columns: new[] { "CustomerId", "Currency" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_AccountId",
                schema: "payments",
                table: "Deposits",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalAccounts_CustomerId_Currency",
                schema: "payments",
                table: "WithdrawalAccounts",
                columns: new[] { "CustomerId", "Currency" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Withdrawals_AccountId",
                schema: "payments",
                table: "Withdrawals",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deposits",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "Withdrawals",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "DepositAccounts",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "WithdrawalAccounts",
                schema: "payments");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "payments");
        }
    }
}
