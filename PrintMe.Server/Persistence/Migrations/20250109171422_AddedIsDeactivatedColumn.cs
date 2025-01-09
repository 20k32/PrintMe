using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PrintMe.Server.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "user_phone_number_key",
                table: "user");

            migrationBuilder.AddColumn<bool>(
                name: "is_deactivated",
                table: "printer",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "print_order_status",
                columns: new[] { "print_order_status_id", "status" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Declined" },
                    { 3, "Started" },
                    { 4, "Aborted" },
                    { 5, "Archived" }
                });

            migrationBuilder.InsertData(
                table: "print_order_status_reason",
                columns: new[] { "print_order_status_reason_id", "reason" },
                values: new object[,]
                {
                    { 1, "Inappropriate" },
                    { 2, "OffensiveContent" },
                    { 3, "AbsentMaterials" },
                    { 4, "QualityConcerns" }
                });

            migrationBuilder.InsertData(
                table: "request_status",
                columns: new[] { "request_status_id", "status" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Approved" },
                    { 3, "Declined" }
                });

            migrationBuilder.InsertData(
                table: "request_status_reason",
                columns: new[] { "request_status_reason_id", "reason" },
                values: new object[,]
                {
                    { 1, "Inappropriate" },
                    { 2, "OffensiveContent" },
                    { 3, "SystemAbuse" }
                });

            migrationBuilder.InsertData(
                table: "request_type",
                columns: new[] { "request_type_id", "type" },
                values: new object[,]
                {
                    { 1, "PrinterApplication" },
                    { 2, "PrinterDescriptionChanging" },
                    { 3, "UserReport" },
                    { 4, "AccountDeletion" }
                });

            migrationBuilder.InsertData(
                table: "user_role",
                columns: new[] { "UserRoleId", "user_role_name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "PrinterOwner" }
                });

            migrationBuilder.InsertData(
                table: "user_status",
                columns: new[] { "user_status_id", "status" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Inactive" },
                    { 3, "Blocked" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "print_order_status",
                keyColumn: "print_order_status_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "print_order_status",
                keyColumn: "print_order_status_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "print_order_status",
                keyColumn: "print_order_status_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "print_order_status",
                keyColumn: "print_order_status_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "print_order_status",
                keyColumn: "print_order_status_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "print_order_status_reason",
                keyColumn: "print_order_status_reason_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "print_order_status_reason",
                keyColumn: "print_order_status_reason_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "print_order_status_reason",
                keyColumn: "print_order_status_reason_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "print_order_status_reason",
                keyColumn: "print_order_status_reason_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "request_status",
                keyColumn: "request_status_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "request_status",
                keyColumn: "request_status_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "request_status",
                keyColumn: "request_status_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "request_status_reason",
                keyColumn: "request_status_reason_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "request_status_reason",
                keyColumn: "request_status_reason_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "request_status_reason",
                keyColumn: "request_status_reason_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "request_type",
                keyColumn: "request_type_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "request_type",
                keyColumn: "request_type_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "request_type",
                keyColumn: "request_type_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "request_type",
                keyColumn: "request_type_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "user_role",
                keyColumn: "UserRoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "user_role",
                keyColumn: "UserRoleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "is_deactivated",
                table: "printer");

            migrationBuilder.CreateIndex(
                name: "user_phone_number_key",
                table: "user",
                column: "phone_number",
                unique: true);
        }
    }
}
