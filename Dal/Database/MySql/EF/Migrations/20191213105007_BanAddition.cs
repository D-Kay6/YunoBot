using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dal.Database.MySql.EF.Migrations
{
    public partial class BanAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_GeneratedChannel_ChannelId_ServerId",
                table: "GeneratedChannel");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ban",
                columns: table => new
                {
                    UserId = table.Column<ulong>(nullable: false),
                    ServerId = table.Column<ulong>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Reason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ban", x => new { x.UserId, x.ServerId });
                    table.ForeignKey(
                        name: "FK_Ban_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ban_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleIgnore_UserId",
                table: "RoleIgnore",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ban_ServerId",
                table: "Ban",
                column: "ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleIgnore_User_UserId",
                table: "RoleIgnore",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleIgnore_User_UserId",
                table: "RoleIgnore");

            migrationBuilder.DropTable(
                name: "Ban");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_RoleIgnore_UserId",
                table: "RoleIgnore");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_GeneratedChannel_ChannelId_ServerId",
                table: "GeneratedChannel",
                columns: new[] { "ChannelId", "ServerId" });
        }
    }
}
