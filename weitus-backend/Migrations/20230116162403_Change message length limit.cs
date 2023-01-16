using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace weitus_backend.Migrations
{
    public partial class Changemessagelengthlimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "CHAT_MESSAGES",
                type: "NVARCHAR2(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(500)",
                oldMaxLength: 500);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "CHAT_MESSAGES",
                type: "NVARCHAR2(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)",
                oldMaxLength: 2000);
        }
    }
}
