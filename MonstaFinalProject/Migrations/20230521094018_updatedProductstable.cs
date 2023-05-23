using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonstaFinalProject.Migrations
{
    public partial class updatedProductstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Shipping",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shipping",
                table: "Products");
        }
    }
}
