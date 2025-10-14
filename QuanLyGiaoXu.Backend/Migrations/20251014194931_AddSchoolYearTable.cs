using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyGiaoXu.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddSchoolYearTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchoolYear",
                table: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "SchoolYearId",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SchoolYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolYears", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolYearId",
                table: "Classes",
                column: "SchoolYearId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolYears_Year",
                table: "SchoolYears",
                column: "Year",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_SchoolYears_SchoolYearId",
                table: "Classes",
                column: "SchoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_SchoolYears_SchoolYearId",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "SchoolYears");

            migrationBuilder.DropIndex(
                name: "IX_Classes_SchoolYearId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "SchoolYearId",
                table: "Classes");

            migrationBuilder.AddColumn<string>(
                name: "SchoolYear",
                table: "Classes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
