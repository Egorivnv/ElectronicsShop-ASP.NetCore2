using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ElectronicsShop.Migrations
{
    public partial class OrderConsist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "GiftWrap",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Line2",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Line3",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Orders",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Line1",
                table: "Orders",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Orders",
                newName: "Address");

            migrationBuilder.AlterColumn<bool>(
                name: "Shipped",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Orders",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Orders",
                newName: "Line1");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Orders",
                newName: "Country");

            migrationBuilder.AlterColumn<bool>(
                name: "Shipped",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Orders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "GiftWrap",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Line2",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Line3",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Orders",
                nullable: true);
        }
    }
}
