using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RekrutacjaApp.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarLicense = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CustomAttributes",
                columns: table => new
                {
                    CustomAttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomAttributes", x => x.CustomAttributeId);
                    table.ForeignKey(
                        name: "FK_CustomAttributes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "CarLicense", "Gender", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, new DateTime(1996, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mężczyzna", "Dawid", "Mroczkowski" },
                    { 2, new DateTime(1997, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mężczyzna", "Adam", "Nowak" },
                    { 3, new DateTime(1986, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Mężczyzna", "Jan", "Kowalski" },
                    { 4, new DateTime(1979, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Kobieta", "Karolina", "Szpak" },
                    { 5, new DateTime(1944, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Kobieta", "Wiktoria", "Kowalska" },
                    { 6, new DateTime(1990, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Mężczyzna", "Zbigniew", "Stawik" }
                });

            migrationBuilder.InsertData(
                table: "CustomAttributes",
                columns: new[] { "CustomAttributeId", "Name", "UserId", "Value" },
                values: new object[,]
                {
                    { 1, "Numer buta", 1, "43" },
                    { 2, "Kolor włosów", 2, "Czarne" },
                    { 3, "Kolor kurtki", 2, "Niebieski" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributes_UserId",
                table: "CustomAttributes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomAttributes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
