using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class change_ticketTypes_column_name_v02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                schema: "events",
                table: "Tickets");

            migrationBuilder.RenameTable(
                name: "Tickets",
                schema: "events",
                newName: "TicketTypes",
                newSchema: "events");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketTypes",
                schema: "events",
                table: "TicketTypes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketTypes",
                schema: "events",
                table: "TicketTypes");

            migrationBuilder.RenameTable(
                name: "TicketTypes",
                schema: "events",
                newName: "Tickets",
                newSchema: "events");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                schema: "events",
                table: "Tickets",
                column: "Id");
        }
    }
}
