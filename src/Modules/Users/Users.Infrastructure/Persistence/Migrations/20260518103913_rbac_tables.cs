using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Users.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class rbac_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "users",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "users",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                schema: "users",
                columns: table => new
                {
                    PermissionCode = table.Column<string>(type: "character varying(100)", nullable: false),
                    RoleName = table.Column<string>(type: "character varying(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionCode, x.RoleName });
                    table.ForeignKey(
                        name: "FK_PermissionRole_Permission_PermissionCode",
                        column: x => x.PermissionCode,
                        principalSchema: "users",
                        principalTable: "Permission",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_Role_RoleName",
                        column: x => x.RoleName,
                        principalSchema: "users",
                        principalTable: "Role",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                schema: "users",
                columns: table => new
                {
                    RolesName = table.Column<string>(type: "character varying(50)", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesName, x.UserId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Role_RolesName",
                        column: x => x.RolesName,
                        principalSchema: "users",
                        principalTable: "Role",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "users",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "users",
                table: "Permission",
                column: "Code",
                values: new object[]
                {
                    "carts:add",
                    "carts:read",
                    "carts:remove",
                    "categories:read",
                    "categories:update",
                    "event-statistics:read",
                    "events:read",
                    "events:search",
                    "events:update",
                    "orders:create",
                    "orders:read",
                    "ticket-types:read",
                    "ticket-types:update",
                    "tickets:check-in",
                    "tickets:read",
                    "users:read",
                    "users:update"
                });

            migrationBuilder.InsertData(
                schema: "users",
                table: "Role",
                column: "Name",
                values: new object[]
                {
                    "Administrator",
                    "Member"
                });

            migrationBuilder.InsertData(
                schema: "users",
                table: "PermissionRole",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[,]
                {
                    { "carts:add", "Administrator" },
                    { "carts:add", "Member" },
                    { "carts:read", "Administrator" },
                    { "carts:read", "Member" },
                    { "carts:remove", "Administrator" },
                    { "carts:remove", "Member" },
                    { "categories:read", "Administrator" },
                    { "categories:update", "Administrator" },
                    { "event-statistics:read", "Administrator" },
                    { "events:read", "Administrator" },
                    { "events:search", "Administrator" },
                    { "events:search", "Member" },
                    { "events:update", "Administrator" },
                    { "orders:create", "Administrator" },
                    { "orders:create", "Member" },
                    { "orders:read", "Administrator" },
                    { "orders:read", "Member" },
                    { "ticket-types:read", "Administrator" },
                    { "ticket-types:read", "Member" },
                    { "ticket-types:update", "Administrator" },
                    { "tickets:check-in", "Administrator" },
                    { "tickets:check-in", "Member" },
                    { "tickets:read", "Administrator" },
                    { "tickets:read", "Member" },
                    { "users:read", "Administrator" },
                    { "users:read", "Member" },
                    { "users:update", "Administrator" },
                    { "users:update", "Member" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RoleName",
                schema: "users",
                table: "PermissionRole",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UserId",
                schema: "users",
                table: "RoleUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionRole",
                schema: "users");

            migrationBuilder.DropTable(
                name: "RoleUser",
                schema: "users");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "users");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "users");
        }
    }
}
