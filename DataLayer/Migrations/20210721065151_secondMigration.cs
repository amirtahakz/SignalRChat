using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class secondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateData",
                table: "Users",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreateData",
                table: "UserRoles",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreateData",
                table: "Roles",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreateData",
                table: "RolePermissions",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreateData",
                table: "Chats",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreateData",
                table: "ChatGroups",
                newName: "CreateDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Users",
                newName: "CreateData");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "UserRoles",
                newName: "CreateData");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Roles",
                newName: "CreateData");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "RolePermissions",
                newName: "CreateData");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Chats",
                newName: "CreateData");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "ChatGroups",
                newName: "CreateData");
        }
    }
}
