using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_users_module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                schema: "users",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "users",
                newName: "Users",
                newSchema: "users");

            migrationBuilder.RenameColumn(
                name: "email",
                schema: "users",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "users",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "last_name",
                schema: "users",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                schema: "users",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                schema: "users",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "users",
                table: "Users",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "users",
                newName: "users",
                newSchema: "users");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "users",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "users",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "users",
                table: "users",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "users",
                table: "users",
                newName: "first_name");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                schema: "users",
                table: "users",
                newName: "ix_users_email");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                schema: "users",
                table: "users",
                column: "id");
        }
    }
}
