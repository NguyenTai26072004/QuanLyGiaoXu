using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyGiaoXu.Backend.Migrations
{
    /// <inheritdoc />
    public partial class RestructureAttendanceModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Session",
                table: "Attendances");

            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AttendanceSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassSchedules",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSchedules", x => new { x.ClassId, x.SessionId });
                    table.ForeignKey(
                        name: "FK_ClassSchedules_AttendanceSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "AttendanceSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSchedules_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SessionId",
                table: "Attendances",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceSessions_Name",
                table: "AttendanceSessions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_SessionId",
                table: "ClassSchedules",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AttendanceSessions_SessionId",
                table: "Attendances",
                column: "SessionId",
                principalTable: "AttendanceSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AttendanceSessions_SessionId",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "ClassSchedules");

            migrationBuilder.DropTable(
                name: "AttendanceSessions");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_SessionId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Attendances");

            migrationBuilder.AddColumn<string>(
                name: "Session",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
