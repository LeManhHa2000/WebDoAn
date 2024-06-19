using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebDoAn.Migrations
{
    public partial class lan3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productImg");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Evaluate",
                table: "product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "product",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "user");

            migrationBuilder.DropColumn(
                name: "Evaluate",
                table: "product");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "product");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "product");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "product");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "product");

            migrationBuilder.CreateTable(
                name: "productImg",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ImgSrc = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productImg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_productImg_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_productImg_ProductId",
                table: "productImg",
                column: "ProductId");
        }
    }
}
