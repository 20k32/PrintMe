using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrintMe.Server.Migrations
{
    /// <inheritdoc />
    public partial class MessageIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "message_pkey",
                table: "message");

            migrationBuilder.RenameTable(
                name: "message",
                newName: "Messages");

            migrationBuilder.RenameIndex(
                name: "IX_message_sender_id",
                table: "Messages",
                newName: "IX_Messages_sender_id");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "print_order",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "printer_id",
                table: "print_order",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sender_id",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "message_id",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "message_pkey",
                table: "Messages",
                column: "message_id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_message_id",
                table: "Messages",
                column: "message_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "message_pkey",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_message_id",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "message_id",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "message");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_sender_id",
                table: "message",
                newName: "IX_message_sender_id");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "print_order",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "printer_id",
                table: "print_order",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "sender_id",
                table: "message",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "message_pkey",
                table: "message",
                columns: new[] { "chat_id", "send_time" });
        }
    }
}
