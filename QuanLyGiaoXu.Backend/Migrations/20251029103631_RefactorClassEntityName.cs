using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyGiaoXu.Backend.Migrations
{
    /// <inheritdoc />
    public partial class RefactorClassEntityName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Classes",
                newName: "ClassName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "Classes",
                newName: "Name");
        }
    }
}
