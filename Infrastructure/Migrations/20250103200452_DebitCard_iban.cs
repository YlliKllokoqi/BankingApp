using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DebitCard_iban : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebitCard_AspNetUsers_OwnerId",
                table: "DebitCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DebitCard",
                table: "DebitCard");

            migrationBuilder.RenameTable(
                name: "DebitCard",
                newName: "DebitCards");

            migrationBuilder.RenameIndex(
                name: "IX_DebitCard_OwnerId",
                table: "DebitCards",
                newName: "IX_DebitCards_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "DebitCards",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DebitCards",
                table: "DebitCards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebitCards_AspNetUsers_OwnerId",
                table: "DebitCards",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebitCards_AspNetUsers_OwnerId",
                table: "DebitCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DebitCards",
                table: "DebitCards");

            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "DebitCards");

            migrationBuilder.RenameTable(
                name: "DebitCards",
                newName: "DebitCard");

            migrationBuilder.RenameIndex(
                name: "IX_DebitCards_OwnerId",
                table: "DebitCard",
                newName: "IX_DebitCard_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DebitCard",
                table: "DebitCard",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebitCard_AspNetUsers_OwnerId",
                table: "DebitCard",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
