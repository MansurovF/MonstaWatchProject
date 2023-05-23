using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonstaFinalProject.Migrations
{
    public partial class updatedBasketstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Shipping",
                table: "Baskets",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shipping",
                table: "Baskets");
        }
    }
}
