using Microsoft.EntityFrameworkCore.Migrations;

namespace Dal.Database.MySql.EF.Migrations
{
    public partial class RoleRelationFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_DynamicRole_Roles",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_ReactionRole_Roles",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_Roles",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Role");

            migrationBuilder.CreateTable(
                name: "DynamicRoleData",
                columns: table => new
                {
                    DynamicRoleId = table.Column<ulong>(nullable: false),
                    RoleId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicRoleData", x => new { x.RoleId, x.DynamicRoleId });
                    table.ForeignKey(
                        name: "FK_DynamicRoleData_DynamicRole_DynamicRoleId",
                        column: x => x.DynamicRoleId,
                        principalTable: "DynamicRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReactionRoleData",
                columns: table => new
                {
                    ReactionRoleId = table.Column<ulong>(nullable: false),
                    RoleId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionRoleData", x => new { x.RoleId, x.ReactionRoleId });
                    table.ForeignKey(
                        name: "FK_ReactionRoleData_ReactionRole_ReactionRoleId",
                        column: x => x.ReactionRoleId,
                        principalTable: "ReactionRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicRole_Type_Status",
                table: "DynamicRole",
                columns: new[] { "Type", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicRoleData_DynamicRoleId",
                table: "DynamicRoleData",
                column: "DynamicRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleData_ReactionRoleId",
                table: "ReactionRoleData",
                column: "ReactionRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicRoleData");

            migrationBuilder.DropTable(
                name: "ReactionRoleData");

            migrationBuilder.DropIndex(
                name: "IX_DynamicRole_Type_Status",
                table: "DynamicRole");

            migrationBuilder.AddColumn<ulong>(
                name: "Roles",
                table: "Role",
                type: "bigint unsigned",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Roles",
                table: "Role",
                column: "Roles");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_DynamicRole_Roles",
                table: "Role",
                column: "Roles",
                principalTable: "DynamicRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_ReactionRole_Roles",
                table: "Role",
                column: "Roles",
                principalTable: "ReactionRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
