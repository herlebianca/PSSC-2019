using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPlanner.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTask_User_Userid",
                table: "MyTask");

            migrationBuilder.DropForeignKey(
                name: "FK_MyTask_User_Userid1",
                table: "MyTask");

            migrationBuilder.DropIndex(
                name: "IX_MyTask_Userid",
                table: "MyTask");

            migrationBuilder.DropIndex(
                name: "IX_MyTask_Userid1",
                table: "MyTask");

            migrationBuilder.DropColumn(
                name: "name",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Project",
                table: "MyTask");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "MyTask");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "MyTask");

            migrationBuilder.DropColumn(
                name: "Userid1",
                table: "MyTask");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "picture_path",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rating",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "MyTask",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "MyTask",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Movement",
                table: "MyTask",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Physical_Effort",
                table: "MyTask",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "MyTask",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tag",
                table: "MyTask",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Urgency",
                table: "MyTask",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "User");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "User");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "User");

            migrationBuilder.DropColumn(
                name: "picture_path",
                table: "User");

            migrationBuilder.DropColumn(
                name: "rating",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "MyTask");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "MyTask");

            migrationBuilder.DropColumn(
                name: "Movement",
                table: "MyTask");

            migrationBuilder.DropColumn(
                name: "Physical_Effort",
                table: "MyTask");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "MyTask");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "MyTask");

            migrationBuilder.DropColumn(
                name: "Urgency",
                table: "MyTask");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project",
                table: "MyTask",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "MyTask",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Userid",
                table: "MyTask",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Userid1",
                table: "MyTask",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyTask_Userid",
                table: "MyTask",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_MyTask_Userid1",
                table: "MyTask",
                column: "Userid1");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTask_User_Userid",
                table: "MyTask",
                column: "Userid",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MyTask_User_Userid1",
                table: "MyTask",
                column: "Userid1",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
