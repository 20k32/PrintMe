using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrintMe.Server.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "user",
                newName: "salt");

            migrationBuilder.AlterColumn<string>(
                name: "salt",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "user_role_id",
                table: "user",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_role_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_role_pkey", x => x.UserRoleId);
                });

            migrationBuilder.CreateIndex(
                name: "idx_user_role_id",
                table: "user",
                column: "user_role_id");

            migrationBuilder.AddForeignKey(
                name: "user_user_role_id_fkey",
                table: "user",
                column: "user_role_id",
                principalTable: "user_role",
                principalColumn: "UserRoleId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_user_role_id_fkey",
                table: "user");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropIndex(
                name: "idx_user_role_id",
                table: "user");

            migrationBuilder.DropColumn(
                name: "user_role_id",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "salt",
                table: "user",
                newName: "PasswordSalt");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordSalt",
                table: "user",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
