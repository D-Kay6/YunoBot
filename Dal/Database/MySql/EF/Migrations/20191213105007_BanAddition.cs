using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Dal.Database.MySql.EF.Migrations
{
    public partial class BanAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                "AK_GeneratedChannel_ChannelId_ServerId",
                "GeneratedChannel");

            migrationBuilder.CreateTable(
                "User",
                table => new
                {
                    Id = table.Column<ulong>()
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_User", x => x.Id); });

            migrationBuilder.CreateTable(
                "Ban",
                table => new
                {
                    UserId = table.Column<ulong>(),
                    ServerId = table.Column<ulong>(),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Reason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ban", x => new {x.UserId, x.ServerId});
                    table.ForeignKey(
                        "FK_Ban_Server_ServerId",
                        x => x.ServerId,
                        "Server",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Ban_User_UserId",
                        x => x.UserId,
                        "User",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_RoleIgnore_UserId",
                "RoleIgnore",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_Ban_ServerId",
                "Ban",
                "ServerId");

            migrationBuilder.AddForeignKey(
                "FK_RoleIgnore_User_UserId",
                "RoleIgnore",
                "UserId",
                "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_RoleIgnore_User_UserId",
                "RoleIgnore");

            migrationBuilder.DropTable(
                "Ban");

            migrationBuilder.DropTable(
                "User");

            migrationBuilder.DropIndex(
                "IX_RoleIgnore_UserId",
                "RoleIgnore");

            migrationBuilder.AddUniqueConstraint(
                "AK_GeneratedChannel_ChannelId_ServerId",
                "GeneratedChannel",
                new[] {"ChannelId", "ServerId"});
        }
    }
}