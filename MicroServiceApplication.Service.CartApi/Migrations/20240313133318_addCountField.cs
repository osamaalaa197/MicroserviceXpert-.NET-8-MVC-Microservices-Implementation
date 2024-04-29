using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroServiceApplication.Service.CartApi.Migrations
{
    /// <inheritdoc />
    public partial class addCountField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "CartDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "CartDetails");
        }
    }
}
