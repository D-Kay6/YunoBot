namespace Dal.Database.MySql.EF.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CustomCommands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "CustomCommand",
                table => new
                {
                    ServerId = table.Column<ulong>(),
                    Command = table.Column<string>(maxLength: 50),
                    Response = table.Column<string>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomCommand", x => new {x.ServerId, x.Command});
                    table.ForeignKey(
                        "FK_CustomCommand_Server_ServerId",
                        x => x.ServerId,
                        "Server",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "CustomCommand");
        }
    }
}