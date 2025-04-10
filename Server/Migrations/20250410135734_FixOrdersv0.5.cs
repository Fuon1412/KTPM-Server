using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class FixOrdersv05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderItemModel");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemModel_ProductId",
                table: "OrderItemModel",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemModel_Products_ProductId",
                table: "OrderItemModel",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemModel_Products_ProductId",
                table: "OrderItemModel");

            migrationBuilder.DropIndex(
                name: "IX_OrderItemModel_ProductId",
                table: "OrderItemModel");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "OrderItemModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
