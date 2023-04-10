using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultEventFormats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "79cc04a4-f273-46ac-934e-92eb8a18e356");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7eecef36-494b-4e21-84ac-dfbdc8b40651");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e6ac900-2c68-4745-841a-1a5e2ad66177", null, "admin", "ADMIN" },
                    { "5e150671-eb15-4aa6-9cd8-a23396e09937", null, "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "EventFormats",
                columns: new[] { "Id", "Format" },
                values: new object[,]
                {
                    { 1, "Online" },
                    { 2, "Ofline" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e6ac900-2c68-4745-841a-1a5e2ad66177");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e150671-eb15-4aa6-9cd8-a23396e09937");

            migrationBuilder.DeleteData(
                table: "EventFormats",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EventFormats",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "79cc04a4-f273-46ac-934e-92eb8a18e356", null, "admin", "ADMIN" },
                    { "7eecef36-494b-4e21-84ac-dfbdc8b40651", null, "user", "USER" }
                });
        }
    }
}
