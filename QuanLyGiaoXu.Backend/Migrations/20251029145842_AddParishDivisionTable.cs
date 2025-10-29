using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyGiaoXu.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddParishDivisionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParishDivision",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "ParishDivisionId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ParishDivisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParishDivisions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_ParishDivisionId",
                table: "Students",
                column: "ParishDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_ParishDivisions_Name",
                table: "ParishDivisions",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_ParishDivisions_ParishDivisionId",
                table: "Students",
                column: "ParishDivisionId",
                principalTable: "ParishDivisions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_ParishDivisions_ParishDivisionId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "ParishDivisions");

            migrationBuilder.DropIndex(
                name: "IX_Students_ParishDivisionId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ParishDivisionId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "ParishDivision",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
