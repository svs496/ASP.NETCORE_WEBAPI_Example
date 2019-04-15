using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.DataLayer.Migrations
{
    public partial class NewColumnModifyDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "Tasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "Tasks");
        }
    }
}
