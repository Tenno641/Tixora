using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tickets.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class change_schema_name_tickets_module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tickets");

            migrationBuilder.RenameTable(
                name: "Customers",
                schema: "Tickets",
                newName: "Customers",
                newSchema: "tickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Tickets");

            migrationBuilder.RenameTable(
                name: "Customers",
                schema: "tickets",
                newName: "Customers",
                newSchema: "Tickets");
        }
    }
}
