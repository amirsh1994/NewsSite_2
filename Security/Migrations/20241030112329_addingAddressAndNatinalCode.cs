using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Security.Migrations
{
    /// <inheritdoc />
    public partial class addingAddressAndNatinalCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NationalCode",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAddress",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserAddress",
                table: "AspNetUsers");
        }
    }
}
