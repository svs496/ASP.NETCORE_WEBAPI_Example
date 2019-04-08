using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.DataLayer.Migrations
{
    public partial class ParentTaskNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ParentTaskId",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ParentTaskId",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
