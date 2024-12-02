using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FintechApp.Migrations
{
    /// <inheritdoc />
    public partial class emailaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Transactions",
                newName: "EmailAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Transactions",
                newName: "Email");
        }
    }
}
