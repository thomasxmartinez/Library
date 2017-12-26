using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LibraryData.Migrations
{
    public partial class hopethis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patrons_CheckoutHistoryId",
                table: "Patrons");

            migrationBuilder.CreateIndex(
                name: "IX_Patrons_CheckoutHistoryId",
                table: "Patrons",
                column: "CheckoutHistoryId",
                unique: true,
                filter: "[CheckoutHistoryId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patrons_CheckoutHistoryId",
                table: "Patrons");

            migrationBuilder.CreateIndex(
                name: "IX_Patrons_CheckoutHistoryId",
                table: "Patrons",
                column: "CheckoutHistoryId");
        }
    }
}
