using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class FixOrdersv02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemModel_Orders_OrderId",
                table: "OrderItemModel");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemModel_Products_ProductId",
                table: "OrderItemModel");

            migrationBuilder.DropIndex(
                name: "IX_OrderItemModel_OrderId",
                table: "OrderItemModel");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderItemModel");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrderItemModel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderModelId",
                table: "OrderItemModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemModel_OrderModelId",
                table: "OrderItemModel",
                column: "OrderModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemModel_Orders_OrderModelId",
                table: "OrderItemModel",
                column: "OrderModelId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemModel_Products_ProductId",
                table: "OrderItemModel",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemModel_Orders_OrderModelId",
                table: "OrderItemModel");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemModel_Products_ProductId",
                table: "OrderItemModel");

            migrationBuilder.DropIndex(
                name: "IX_OrderItemModel_OrderModelId",
                table: "OrderItemModel");

            migrationBuilder.DropColumn(
                name: "OrderModelId",
                table: "OrderItemModel");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrderItemModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "OrderItemModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemModel_OrderId",
                table: "OrderItemModel",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemModel_Orders_OrderId",
                table: "OrderItemModel",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemModel_Products_ProductId",
                table: "OrderItemModel",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
