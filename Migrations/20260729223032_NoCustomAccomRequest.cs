using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wedding.Migrations
{
    /// <inheritdoc />
    public partial class NoCustomAccomRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestAccomodation",
                table: "Parties");

            migrationBuilder.AddColumn<bool>(
                name: "Housed",
                table: "Parties",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Housed",
                table: "Parties");

            migrationBuilder.AddColumn<bool>(
                name: "RequestAccomodation",
                table: "Parties",
                type: "boolean",
                nullable: true);
        }
    }
}
