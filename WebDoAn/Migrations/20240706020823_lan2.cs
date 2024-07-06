using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDoAn.Migrations
{
    public partial class lan2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressReceive",
                table: "order",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressReceive",
                table: "order");
        }
    }
}
