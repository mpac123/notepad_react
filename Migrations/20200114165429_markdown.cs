using Microsoft.EntityFrameworkCore.Migrations;

namespace notepad_react.Migrations
{
    public partial class markdown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMarkdown",
                table: "Notes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMarkdown",
                table: "Notes");
        }
    }
}
