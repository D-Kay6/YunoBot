using Microsoft.EntityFrameworkCore.Migrations;

namespace Dal.Database.MySql.EF.Migrations
{
    public partial class CustomCommands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomCommand",
                columns: table => new
                {
                    ServerId = table.Column<ulong>(nullable: false),
                    Command = table.Column<string>(maxLength: 50, nullable: false),
                    Response = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomCommand", x => new { x.ServerId, x.Command });
                    table.ForeignKey(
                        name: "FK_CustomCommand_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomCommand");
        }
    }
}