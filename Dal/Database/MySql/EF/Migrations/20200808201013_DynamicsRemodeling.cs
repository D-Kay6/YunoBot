using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dal.Database.MySql.EF.Migrations
{
    public partial class DynamicsRemodeling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneratedChannel_AutoChannel_ServerId",
                table: "GeneratedChannel");

            migrationBuilder.DropTable(
                name: "AutoChannel");

            migrationBuilder.DropTable(
                name: "AutoRole");

            migrationBuilder.DropTable(
                name: "PermaChannel");

            migrationBuilder.DropTable(
                name: "PermaRole");

            migrationBuilder.DropTable(
                name: "RoleIgnore");

            migrationBuilder.AlterColumn<string>(
                name: "Response",
                table: "CustomCommand",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Ban",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DynamicChannel",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServerId = table.Column<ulong>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Prefix = table.Column<string>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicChannel_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicRole",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServerId = table.Column<ulong>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicRole_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicRoleIgnore",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(nullable: false),
                    UserId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicRoleIgnore", x => new { x.ServerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_DynamicRoleIgnore_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DynamicRoleIgnore_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReactionRole",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServerId = table.Column<ulong>(nullable: false),
                    Message = table.Column<ulong>(nullable: false),
                    Reaction = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactionRole_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Roles = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_DynamicRole_Roles",
                        column: x => x.Roles,
                        principalTable: "DynamicRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_ReactionRole_Roles",
                        column: x => x.Roles,
                        principalTable: "ReactionRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicChannel_ServerId_Type",
                table: "DynamicChannel",
                columns: new[] { "ServerId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicRole_ServerId",
                table: "DynamicRole",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicRoleIgnore_UserId",
                table: "DynamicRoleIgnore",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRole_ServerId",
                table: "ReactionRole",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Roles",
                table: "Role",
                column: "Roles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicChannel");

            migrationBuilder.DropTable(
                name: "DynamicRoleIgnore");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "DynamicRole");

            migrationBuilder.DropTable(
                name: "ReactionRole");

            migrationBuilder.AlterColumn<string>(
                name: "Response",
                table: "CustomCommand",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Ban",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "AutoChannel",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: false),
                    Prefix = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: false)
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
                    ServerId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Prefix = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: false)
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
                name: "PermaChannel",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: false),
                    Prefix = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: false)
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
                    ServerId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Prefix = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: false)
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
                    ServerId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_RoleIgnore_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleIgnore_UserId",
                table: "RoleIgnore",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneratedChannel_AutoChannel_ServerId",
                table: "GeneratedChannel",
                column: "ServerId",
                principalTable: "AutoChannel",
                principalColumn: "ServerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
