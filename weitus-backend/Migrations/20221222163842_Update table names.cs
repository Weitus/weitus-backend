using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace weitus_backend.Migrations
{
    public partial class Updatetablenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_AspNetUsers_ChatterId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ChatBots_BotId",
                table: "ChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatBots",
                table: "ChatBots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "ChatMessages",
                newName: "CHAT_MESSAGES");

            migrationBuilder.RenameTable(
                name: "ChatBots",
                newName: "CHAT_BOTS");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "ASP_IDENTITY_TOKENS");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "ASP_IDENTITY_USERS");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "ASP_IDENTITY_LOGINS");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "ASP_IDENTITY_CLAIMS");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_ChatterId",
                table: "CHAT_MESSAGES",
                newName: "IX_CHAT_MESSAGES_ChatterId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_BotId",
                table: "CHAT_MESSAGES",
                newName: "IX_CHAT_MESSAGES_BotId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "ASP_IDENTITY_LOGINS",
                newName: "IX_ASP_IDENTITY_LOGINS_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "ASP_IDENTITY_CLAIMS",
                newName: "IX_ASP_IDENTITY_CLAIMS_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CHAT_MESSAGES",
                table: "CHAT_MESSAGES",
                column: "ChatMessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CHAT_BOTS",
                table: "CHAT_BOTS",
                column: "ChatBotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ASP_IDENTITY_TOKENS",
                table: "ASP_IDENTITY_TOKENS",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ASP_IDENTITY_USERS",
                table: "ASP_IDENTITY_USERS",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ASP_IDENTITY_LOGINS",
                table: "ASP_IDENTITY_LOGINS",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ASP_IDENTITY_CLAIMS",
                table: "ASP_IDENTITY_CLAIMS",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CHAT_MESSAGES_TimeStamp",
                table: "CHAT_MESSAGES",
                column: "TimeStamp");

            migrationBuilder.AddForeignKey(
                name: "FK_ASP_IDENTITY_CLAIMS_ASP_IDENTITY_USERS_UserId",
                table: "ASP_IDENTITY_CLAIMS",
                column: "UserId",
                principalTable: "ASP_IDENTITY_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ASP_IDENTITY_LOGINS_ASP_IDENTITY_USERS_UserId",
                table: "ASP_IDENTITY_LOGINS",
                column: "UserId",
                principalTable: "ASP_IDENTITY_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ASP_IDENTITY_TOKENS_ASP_IDENTITY_USERS_UserId",
                table: "ASP_IDENTITY_TOKENS",
                column: "UserId",
                principalTable: "ASP_IDENTITY_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CHAT_MESSAGES_ASP_IDENTITY_USERS_ChatterId",
                table: "CHAT_MESSAGES",
                column: "ChatterId",
                principalTable: "ASP_IDENTITY_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CHAT_MESSAGES_CHAT_BOTS_BotId",
                table: "CHAT_MESSAGES",
                column: "BotId",
                principalTable: "CHAT_BOTS",
                principalColumn: "ChatBotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ASP_IDENTITY_CLAIMS_ASP_IDENTITY_USERS_UserId",
                table: "ASP_IDENTITY_CLAIMS");

            migrationBuilder.DropForeignKey(
                name: "FK_ASP_IDENTITY_LOGINS_ASP_IDENTITY_USERS_UserId",
                table: "ASP_IDENTITY_LOGINS");

            migrationBuilder.DropForeignKey(
                name: "FK_ASP_IDENTITY_TOKENS_ASP_IDENTITY_USERS_UserId",
                table: "ASP_IDENTITY_TOKENS");

            migrationBuilder.DropForeignKey(
                name: "FK_CHAT_MESSAGES_ASP_IDENTITY_USERS_ChatterId",
                table: "CHAT_MESSAGES");

            migrationBuilder.DropForeignKey(
                name: "FK_CHAT_MESSAGES_CHAT_BOTS_BotId",
                table: "CHAT_MESSAGES");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CHAT_MESSAGES",
                table: "CHAT_MESSAGES");

            migrationBuilder.DropIndex(
                name: "IX_CHAT_MESSAGES_TimeStamp",
                table: "CHAT_MESSAGES");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CHAT_BOTS",
                table: "CHAT_BOTS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ASP_IDENTITY_USERS",
                table: "ASP_IDENTITY_USERS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ASP_IDENTITY_TOKENS",
                table: "ASP_IDENTITY_TOKENS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ASP_IDENTITY_LOGINS",
                table: "ASP_IDENTITY_LOGINS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ASP_IDENTITY_CLAIMS",
                table: "ASP_IDENTITY_CLAIMS");

            migrationBuilder.RenameTable(
                name: "CHAT_MESSAGES",
                newName: "ChatMessages");

            migrationBuilder.RenameTable(
                name: "CHAT_BOTS",
                newName: "ChatBots");

            migrationBuilder.RenameTable(
                name: "ASP_IDENTITY_USERS",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "ASP_IDENTITY_TOKENS",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "ASP_IDENTITY_LOGINS",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "ASP_IDENTITY_CLAIMS",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameIndex(
                name: "IX_CHAT_MESSAGES_ChatterId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_ChatterId");

            migrationBuilder.RenameIndex(
                name: "IX_CHAT_MESSAGES_BotId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_BotId");

            migrationBuilder.RenameIndex(
                name: "IX_ASP_IDENTITY_LOGINS_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ASP_IDENTITY_CLAIMS_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages",
                column: "ChatMessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatBots",
                table: "ChatBots",
                column: "ChatBotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_AspNetUsers_ChatterId",
                table: "ChatMessages",
                column: "ChatterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ChatBots_BotId",
                table: "ChatMessages",
                column: "BotId",
                principalTable: "ChatBots",
                principalColumn: "ChatBotId");
        }
    }
}
