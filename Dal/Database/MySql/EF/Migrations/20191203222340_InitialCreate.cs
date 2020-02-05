using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dal.Database.MySql.EF.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Server",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Server", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AutoChannel",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(nullable: false),
                    Prefix = table.Column<string>(maxLength: 100, nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoChannel", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_AutoChannel_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutoRole",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(nullable: false),
                    Prefix = table.Column<string>(maxLength: 100, nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoRole", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_AutoRole_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandSetting",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(nullable: false),
                    Prefix = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandSetting", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_CommandSetting_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageSetting",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(nullable: false),
                    Language = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageSetting", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_LanguageSetting_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermaChannel",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(nullable: false),
                    Prefix = table.Column<string>(maxLength: 100, nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermaChannel", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_PermaChannel_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermaRole",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(nullable: false),
                    Prefix = table.Column<string>(maxLength: 100, nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermaRole", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_PermaRole_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleIgnore",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(nullable: false),
                    UserId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleIgnore", x => new { x.ServerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RoleIgnore_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WelcomeMessage",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(nullable: false),
                    ChannelId = table.Column<ulong>(nullable: true),
                    UseImage = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelcomeMessage", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_WelcomeMessage_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedChannel",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(nullable: false),
                    ChannelId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedChannel", x => new { x.ServerId, x.ChannelId });
                    table.UniqueConstraint("AK_GeneratedChannel_ChannelId_ServerId", x => new { x.ChannelId, x.ServerId });
                    table.ForeignKey(
                        name: "FK_GeneratedChannel_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneratedChannel_AutoChannel_ServerId",
                        column: x => x.ServerId,
                        principalTable: "AutoChannel",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoRole");

            migrationBuilder.DropTable(
                name: "CommandSetting");

            migrationBuilder.DropTable(
                name: "GeneratedChannel");

            migrationBuilder.DropTable(
                name: "LanguageSetting");

            migrationBuilder.DropTable(
                name: "PermaChannel");

            migrationBuilder.DropTable(
                name: "PermaRole");

            migrationBuilder.DropTable(
                name: "RoleIgnore");

            migrationBuilder.DropTable(
                name: "WelcomeMessage");

            migrationBuilder.DropTable(
                name: "AutoChannel");

            migrationBuilder.DropTable(
                name: "Server");
        }
    }
}
