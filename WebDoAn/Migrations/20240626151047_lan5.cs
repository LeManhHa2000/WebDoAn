using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebDoAn.Migrations
{
    public partial class lan5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "user");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "product");

            migrationBuilder.DropColumn(
                name: "TypeProduct",
                table: "product");

            migrationBuilder.DropColumn(
                name: "ImgProduct",
                table: "orderDetail");

            migrationBuilder.DropColumn(
                name: "Pay",
                table: "order");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "categorie");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "cart");

            migrationBuilder.RenameColumn(
                name: "SerName",
                table: "user",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "order",
                newName: "UpdateTime");

            migrationBuilder.AddColumn<byte>(
                name: "Gender",
                table: "user",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "user",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "product",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<int>(
                name: "Total",
                table: "orderDetail",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "PaymentMethod",
                table: "order",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "categorie",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "blog",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressName = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_address_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_AddressId",
                table: "order",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_order_UserId",
                table: "order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_address_UserId",
                table: "address",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_order_address_AddressId",
                table: "order",
                column: "AddressId",
                principalTable: "address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_user_UserId",
                table: "order",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_address_AddressId",
                table: "order");

            migrationBuilder.DropForeignKey(
                name: "FK_order_user_UserId",
                table: "order");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropIndex(
                name: "IX_order_AddressId",
                table: "order");

            migrationBuilder.DropIndex(
                name: "IX_order_UserId",
                table: "order");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "user");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "user");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "order");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "order");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "order");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "order");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "categorie");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "blog");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "user",
                newName: "SerName");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "order",
                newName: "PaymentDate");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "product",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "TypeProduct",
                table: "product",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "orderDetail",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "ImgProduct",
                table: "orderDetail",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Pay",
                table: "order",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "categorie",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "cart",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
