namespace Dal.Database.MySql.EF.Migrations
{
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Server",
                table => new
                {
                    Id = table.Column<ulong>()
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100)
                },
                constraints: table => { table.PrimaryKey("PK_Server", x => x.Id); });

            migrationBuilder.CreateTable(
                "AutoChannel",
                table => new
                {
                    ServerId = table.Column<ulong>(),
                    Prefix = table.Column<string>(maxLength: 100),
                    Enabled = table.Column<bool>(),
                    Name = table.Column<string>(maxLength: 100)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoChannel", x => x.ServerId);
                    table.ForeignKey(
                        "FK_AutoChannel_Server_ServerId",
                        x => x.ServerId,
                        "Server",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AutoRole",
                table => new
                {
                    ServerId = table.Column<ulong>(),
                    Prefix = table.Column<string>(maxLength: 100),
                    Enabled = table.Column<bool>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoRole", x => x.ServerId);
                    table.ForeignKey(
                        "FK_AutoRole_Server_ServerId",
                        x => x.ServerId,
                        "Server",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "CommandSetting",
                table => new
                {
                    ServerId = table.Column<ulong>(),
                    Prefix = table.Column<string>(maxLength: 20)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandSetting", x => x.ServerId);
                    table.ForeignKey(
                        "FK_CommandSetting_Server_ServerId",
                        x => x.ServerId,
                        "Server",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "LanguageSetting",
                table => new
                {
                    ServerId = table.Column<ulong>(),
                    Language = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageSetting", x => x.ServerId);
                    table.ForeignKey(
                        "FK_LanguageSetting_Server_ServerId",
                        x => x.ServerId,
                        "Server",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "PermaChannel",
                table => new
                {
                    ServerId = table.Column<ulong>(),
                    Prefix = table.Column<string>(maxLength: 100),
                    Enabled = table.Column<bool>(),
                    Name = table.Column<string>(maxLength: 100)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermaChannel", x => x.ServerId);
                    table.ForeignKey(
                        "FK_PermaChannel_Server_ServerId",
                        x => x.ServerId,
                        "Server",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "PermaRole",
                table => new
                {
                    ServerId = table.Column<ulong>(),
                    Prefix = table.Column<string>(maxLength: 100),
                    Enabled = table.Column<bool>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermaRole", x => x.ServerId);
                    table.ForeignKey(
                        "FK_PermaRole_Server_ServerId",
                        x => x.ServerId,
                        "Server",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "RoleIgnore",
                table => new
                {
                    ServerId = table.Column<ulong>(),
                    UserId = table.Column<ulong>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleIgnore", x => new {x.ServerId, x.UserId});
                    table.ForeignKey(
                        "FK_RoleIgnore_Server_ServerId",
                        x => x.ServerId,
                        "Server",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "WelcomeMessage",
                table => new
                {
                    ServerId = table.Column<ulong>(),
                    ChannelId = table.Column<ulong>(nullable: true),
                    UseImage = table.Column<bool>(),
                    Message = table.Column<string>(maxLength: 2000)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelcomeMessage", x => x.ServerId);
                    table.ForeignKey(
                        "FK_WelcomeMessage_Server_ServerId",
                        x => x.ServerId,
                        "Server",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "GeneratedChannel",
                table => new
                {
                    ServerId = table.Column<ulong>(),
                    ChannelId = table.Column<ulong>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedChannel", x => new {x.ServerId, x.ChannelId});
                    table.UniqueConstraint("AK_GeneratedChannel_ChannelId_ServerId",
                        x => new {x.ChannelId, x.ServerId});
                    table.ForeignKey(
                        "FK_GeneratedChannel_Server_ServerId",
                        x => x.ServerId,
                        "Server",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_GeneratedChannel_AutoChannel_ServerId",
                        x => x.ServerId,
                        "AutoChannel",
                        "ServerId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "AutoRole");

            migrationBuilder.DropTable(
                "CommandSetting");

            migrationBuilder.DropTable(
                "GeneratedChannel");

            migrationBuilder.DropTable(
                "LanguageSetting");

            migrationBuilder.DropTable(
                "PermaChannel");

            migrationBuilder.DropTable(
                "PermaRole");

            migrationBuilder.DropTable(
                "RoleIgnore");

            migrationBuilder.DropTable(
                "WelcomeMessage");

            migrationBuilder.DropTable(
                "AutoChannel");

            migrationBuilder.DropTable(
                "Server");
        }
    }
}