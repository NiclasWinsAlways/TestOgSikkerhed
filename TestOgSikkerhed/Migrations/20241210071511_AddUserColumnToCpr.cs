using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestOgSikkerhed.Migrations
{
    /// <inheritdoc />
    public partial class AddUserColumnToCpr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cpr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CprNr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cpr__3214EC076FC7DAD9", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Todolist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CprId = table.Column<int>(type: "int", nullable: false),
                    Item = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Todolist__3214EC07C3DEF4E0", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Todolist__CprId__38996AB5",
                        column: x => x.CprId,
                        principalTable: "Cpr",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todolist_CprId",
                table: "Todolist",
                column: "CprId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todolist");

            migrationBuilder.DropTable(
                name: "Cpr");
        }
    }
}
