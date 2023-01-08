using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace weitus_backend.Migrations
{
    public partial class DBScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CHAT_BOTS",
                columns: table => new
                {
                    bot_id = table.Column<short>(type: "NUMBER(5)", precision: 5, nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHAT_BOTS", x => x.bot_id);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "NUMBER(8)", precision: 8, nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    username = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    password_hash = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false),
                    password_salt = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "CHAT_MESSAGES",
                columns: table => new
                {
                    chat_message_id = table.Column<int>(type: "NUMBER(10)", precision: 10, nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    timestamp = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    message = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    chatter_id = table.Column<int>(type: "NUMBER(8)", nullable: false),
                    sent_by_bot = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    bot_id = table.Column<short>(type: "NUMBER(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHAT_MESSAGES", x => x.chat_message_id);
                    table.ForeignKey(
                        name: "FK_CHAT_MESSAGES_CHAT_BOTS_bot_id",
                        column: x => x.bot_id,
                        principalTable: "CHAT_BOTS",
                        principalColumn: "bot_id");
                    table.ForeignKey(
                        name: "FK_CHAT_MESSAGES_USERS_chatter_id",
                        column: x => x.chatter_id,
                        principalTable: "USERS",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CHAT_BOTS",
                columns: new[] { "bot_id", "name" },
                values: new object[] { (short)1, "Weituś" });

            migrationBuilder.CreateIndex(
                name: "IX_CHAT_MESSAGES_bot_id",
                table: "CHAT_MESSAGES",
                column: "bot_id");

            migrationBuilder.CreateIndex(
                name: "IX_CHAT_MESSAGES_chatter_id",
                table: "CHAT_MESSAGES",
                column: "chatter_id");

            migrationBuilder.CreateIndex(
                name: "IX_CHAT_MESSAGES_timestamp",
                table: "CHAT_MESSAGES",
                column: "timestamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHAT_MESSAGES");

            migrationBuilder.DropTable(
                name: "CHAT_BOTS");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
