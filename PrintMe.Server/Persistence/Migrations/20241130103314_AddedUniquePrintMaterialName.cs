using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintMe.Server.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniquePrintMaterialName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "user_role",
                columns: new[] { "UserRoleId", "user_role_name" },
                values: new object[] { 3, "Admin" });

            migrationBuilder.CreateIndex(
                name: "idx_print_material_name",
                table: "print_material",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_print_material_name",
                table: "print_material");

            migrationBuilder.DeleteData(
                table: "user_role",
                keyColumn: "UserRoleId",
                keyValue: 3);
        }
    }
}
