using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FintechApp.Migrations
{
    /// <inheritdoc />
    public partial class Schemaupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PIN",
                table: "Transactions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "CardPAN",
                table: "Transactions",
                newName: "MaskedCardPAN");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedCardPAN",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "EncryptedCardPAN",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Transactions",
                newName: "PIN");

            migrationBuilder.RenameColumn(
                name: "MaskedCardPAN",
                table: "Transactions",
                newName: "CardPAN");
        }
    }
}
