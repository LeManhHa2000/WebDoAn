using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDoAn.Migrations
{
    public partial class lan6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "user");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "user",
                type: "text",
                nullable: true);
        }
    }
}
