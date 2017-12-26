using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LibraryData.Migrations
{
    public partial class PatronImageFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheckoutHistoryId",
                table: "Patrons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Patrons",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "Patrons",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Patrons_CheckoutHistoryId",
                table: "Patrons",
                column: "CheckoutHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patrons_CheckoutHistories_CheckoutHistoryId",
                table: "Patrons",
                column: "CheckoutHistoryId",
                principalTable: "CheckoutHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patrons_CheckoutHistories_CheckoutHistoryId",
                table: "Patrons");

            migrationBuilder.DropIndex(
                name: "IX_Patrons_CheckoutHistoryId",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "CheckoutHistoryId",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Patrons");
        }
    }
}
