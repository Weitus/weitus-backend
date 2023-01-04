using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace weitus_backend.Migrations
{
    public partial class Allowmultiplebots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentByBot",
                table: "ChatMessages");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ChatMessages",
                newName: "ChatMessageId");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "ChatMessages",
                type: "NVARCHAR2(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "BotId",
                table: "ChatMessages",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatBots",
                columns: table => new
                {
                    ChatBotId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatBots", x => x.ChatBotId);
                });

            migrationBuilder.InsertData(
                table: "ChatBots",
                columns: new[] { "ChatBotId", "Name" },
                values: new object[] { 1, "Weituś" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_BotId",
                table: "ChatMessages",
                column: "BotId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ChatBots_BotId",
                table: "ChatMessages",
                column: "BotId",
                principalTable: "ChatBots",
                principalColumn: "ChatBotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ChatBots_BotId",
                table: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatBots");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_BotId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "BotId",
                table: "ChatMessages");

            migrationBuilder.RenameColumn(
                name: "ChatMessageId",
                table: "ChatMessages",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "ChatMessages",
                type: "NVARCHAR2(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SentByBot",
                table: "ChatMessages",
                type: "NUMBER(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
