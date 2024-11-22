using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrintMe.Server.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "print_material",
                columns: table => new
                {
                    PrintMaterialId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("print_material_id", x => x.PrintMaterialId);
                });

            migrationBuilder.CreateTable(
                name: "print_order_status",
                columns: table => new
                {
                    print_order_status_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("print_order_status_pkey", x => x.print_order_status_id);
                });

            migrationBuilder.CreateTable(
                name: "print_order_status_reason",
                columns: table => new
                {
                    print_order_status_reason_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reason = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("print_order_status_reason_pkey", x => x.print_order_status_reason_id);
                });

            migrationBuilder.CreateTable(
                name: "printer_model",
                columns: table => new
                {
                    printer_model_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("printer_model_pkey", x => x.printer_model_id);
                });

            migrationBuilder.CreateTable(
                name: "request_status",
                columns: table => new
                {
                    request_status_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("request_status_pkey", x => x.request_status_id);
                });

            migrationBuilder.CreateTable(
                name: "request_status_reason",
                columns: table => new
                {
                    request_status_reason_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reason = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("request_status_reason_pkey", x => x.request_status_reason_id);
                });

            migrationBuilder.CreateTable(
                name: "request_type",
                columns: table => new
                {
                    request_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("request_type_pkey", x => x.request_type_id);
                });

            migrationBuilder.CreateTable(
                name: "user_status",
                columns: table => new
                {
                    user_status_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_status_pkey", x => x.user_status_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    user_status_id = table.Column<int>(type: "integer", nullable: true),
                    should_hide_phone_number = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "user_user_status_id_fkey",
                        column: x => x.user_status_id,
                        principalTable: "user_status",
                        principalColumn: "user_status_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "chat",
                columns: table => new
                {
                    chat_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user1_id = table.Column<int>(type: "integer", nullable: true),
                    user2_id = table.Column<int>(type: "integer", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("chat_pkey", x => x.chat_id);
                    table.ForeignKey(
                        name: "chat_user1_id_fkey",
                        column: x => x.user1_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "chat_user2_id_fkey",
                        column: x => x.user2_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "printer",
                columns: table => new
                {
                    printer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    printer_model_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    min_model_height = table.Column<double>(type: "double precision", nullable: false),
                    min_model_width = table.Column<double>(type: "double precision", nullable: false),
                    max_model_height = table.Column<double>(type: "double precision", nullable: false),
                    max_model_width = table.Column<double>(type: "double precision", nullable: false),
                    location_x = table.Column<double>(type: "double precision", nullable: false),
                    location_y = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("printer_pkey", x => x.printer_id);
                    table.ForeignKey(
                        name: "printer_printer_model_id_fkey",
                        column: x => x.printer_model_id,
                        principalTable: "printer_model",
                        principalColumn: "printer_model_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "printer_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "rating",
                columns: table => new
                {
                    rating_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reviewer = table.Column<int>(type: "integer", nullable: true),
                    target = table.Column<int>(type: "integer", nullable: true),
                    value = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("rating_pkey", x => x.rating_id);
                    table.ForeignKey(
                        name: "rating_reviewer_fkey",
                        column: x => x.reviewer,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "rating_target_fkey",
                        column: x => x.target,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "request",
                columns: table => new
                {
                    request_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_text_data = table.Column<string>(type: "text", nullable: true),
                    user_sender_id = table.Column<int>(type: "integer", nullable: false),
                    request_type_id = table.Column<int>(type: "integer", nullable: false),
                    reported_user_id = table.Column<int>(type: "integer", nullable: true),
                    delete_user_id = table.Column<int>(type: "integer", nullable: true),
                    model_id = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    location_x = table.Column<double>(type: "double precision", nullable: true),
                    location_y = table.Column<double>(type: "double precision", nullable: true),
                    min_model_height = table.Column<double>(type: "double precision", nullable: true),
                    min_model_width = table.Column<double>(type: "double precision", nullable: true),
                    max_model_height = table.Column<double>(type: "double precision", nullable: true),
                    max_model_width = table.Column<double>(type: "double precision", nullable: true),
                    request_status_id = table.Column<int>(type: "integer", nullable: false),
                    request_status_reason_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("request_pkey", x => x.request_id);
                    table.ForeignKey(
                        name: "request_delete_user_id_fkey",
                        column: x => x.delete_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "request_reported_user_id_fkey",
                        column: x => x.reported_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "request_request_status_id_fkey",
                        column: x => x.request_status_id,
                        principalTable: "request_status",
                        principalColumn: "request_status_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "request_request_status_reason_id_fkey",
                        column: x => x.request_status_reason_id,
                        principalTable: "request_status_reason",
                        principalColumn: "request_status_reason_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "request_request_type_id_fkey",
                        column: x => x.request_type_id,
                        principalTable: "request_type",
                        principalColumn: "request_type_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "request_user_sender_id_fkey",
                        column: x => x.user_sender_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    chat_id = table.Column<int>(type: "integer", nullable: false),
                    send_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    sender_id = table.Column<int>(type: "integer", nullable: true),
                    text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("message_pkey", x => new { x.chat_id, x.send_time });
                    table.ForeignKey(
                        name: "message_chat_id_fkey",
                        column: x => x.chat_id,
                        principalTable: "chat",
                        principalColumn: "chat_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "message_sender_id_fkey",
                        column: x => x.sender_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "favourites",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    printer_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("favourites_pkey", x => new { x.user_id, x.printer_id });
                    table.ForeignKey(
                        name: "favourites_printer_id_fkey",
                        column: x => x.printer_id,
                        principalTable: "printer",
                        principalColumn: "printer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "favourites_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "print_materials",
                columns: table => new
                {
                    printer_id = table.Column<int>(type: "integer", nullable: false),
                    material_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("print_materials_pkey", x => new { x.printer_id, x.material_id });
                    table.ForeignKey(
                        name: "print_materials_material_id_fkey",
                        column: x => x.material_id,
                        principalTable: "print_material",
                        principalColumn: "PrintMaterialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "print_materials_printer_id_fkey",
                        column: x => x.printer_id,
                        principalTable: "printer",
                        principalColumn: "printer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "print_order",
                columns: table => new
                {
                    print_order_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    printer_id = table.Column<int>(type: "integer", nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    order_date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    item_link = table.Column<string>(type: "text", nullable: false),
                    item_quantity = table.Column<int>(type: "integer", nullable: true),
                    item_description = table.Column<string>(type: "text", nullable: true),
                    item_material_id = table.Column<int>(type: "integer", nullable: true),
                    print_order_status_id = table.Column<int>(type: "integer", nullable: true),
                    print_order_status_reason_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("print_order_pkey", x => x.print_order_id);
                    table.ForeignKey(
                        name: "print_order_item_material_id_fkey",
                        column: x => x.item_material_id,
                        principalTable: "print_material",
                        principalColumn: "PrintMaterialId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "print_order_print_order_status_id_fkey",
                        column: x => x.print_order_status_id,
                        principalTable: "print_order_status",
                        principalColumn: "print_order_status_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "print_order_print_order_status_reason_id_fkey",
                        column: x => x.print_order_status_reason_id,
                        principalTable: "print_order_status_reason",
                        principalColumn: "print_order_status_reason_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "print_order_printer_id_fkey",
                        column: x => x.printer_id,
                        principalTable: "printer",
                        principalColumn: "printer_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "print_order_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "request_print_materials",
                columns: table => new
                {
                    request_id = table.Column<int>(type: "integer", nullable: false),
                    print_material_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("request_print_materials_pkey", x => new { x.request_id, x.print_material_id });
                    table.ForeignKey(
                        name: "request_print_materials_print_material_id_fkey",
                        column: x => x.print_material_id,
                        principalTable: "print_material",
                        principalColumn: "PrintMaterialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "request_print_materials_request_id_fkey",
                        column: x => x.request_id,
                        principalTable: "request",
                        principalColumn: "request_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_chat_user1_id",
                table: "chat",
                column: "user1_id");

            migrationBuilder.CreateIndex(
                name: "idx_chat_user2_id",
                table: "chat",
                column: "user2_id");

            migrationBuilder.CreateIndex(
                name: "idx_favourites_printer_id",
                table: "favourites",
                column: "printer_id");

            migrationBuilder.CreateIndex(
                name: "idx_favourites_user_id",
                table: "favourites",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_message_chat_id",
                table: "message",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "idx_message_send_time",
                table: "message",
                column: "send_time");

            migrationBuilder.CreateIndex(
                name: "IX_message_sender_id",
                table: "message",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_print_materials_material_id",
                table: "print_materials",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "idx_print_order_order_date",
                table: "print_order",
                column: "order_date");

            migrationBuilder.CreateIndex(
                name: "idx_print_order_printer_id",
                table: "print_order",
                column: "printer_id");

            migrationBuilder.CreateIndex(
                name: "idx_print_order_user_id",
                table: "print_order",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_print_order_item_material_id",
                table: "print_order",
                column: "item_material_id");

            migrationBuilder.CreateIndex(
                name: "IX_print_order_print_order_status_id",
                table: "print_order",
                column: "print_order_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_print_order_print_order_status_reason_id",
                table: "print_order",
                column: "print_order_status_reason_id");

            migrationBuilder.CreateIndex(
                name: "idx_printer_printer_model_id",
                table: "printer",
                column: "printer_model_id");

            migrationBuilder.CreateIndex(
                name: "idx_printer_user_id",
                table: "printer",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_rating_reviewer",
                table: "rating",
                column: "reviewer");

            migrationBuilder.CreateIndex(
                name: "idx_rating_target",
                table: "rating",
                column: "target");

            migrationBuilder.CreateIndex(
                name: "idx_request_request_status_id",
                table: "request",
                column: "request_status_id");

            migrationBuilder.CreateIndex(
                name: "idx_request_request_type_id",
                table: "request",
                column: "request_type_id");

            migrationBuilder.CreateIndex(
                name: "idx_request_user_sender_id",
                table: "request",
                column: "user_sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_delete_user_id",
                table: "request",
                column: "delete_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_reported_user_id",
                table: "request",
                column: "reported_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_request_status_reason_id",
                table: "request",
                column: "request_status_reason_id");

            migrationBuilder.CreateIndex(
                name: "idx_request_print_materials_material_id",
                table: "request_print_materials",
                column: "print_material_id");

            migrationBuilder.CreateIndex(
                name: "idx_request_print_materials_request_id",
                table: "request_print_materials",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_email",
                table: "user",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "idx_user_phone_number",
                table: "user",
                column: "phone_number");

            migrationBuilder.CreateIndex(
                name: "idx_user_status_id",
                table: "user",
                column: "user_status_id");

            migrationBuilder.CreateIndex(
                name: "user_email_key",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_phone_number_key",
                table: "user",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favourites");

            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "print_materials");

            migrationBuilder.DropTable(
                name: "print_order");

            migrationBuilder.DropTable(
                name: "rating");

            migrationBuilder.DropTable(
                name: "request_print_materials");

            migrationBuilder.DropTable(
                name: "chat");

            migrationBuilder.DropTable(
                name: "print_order_status");

            migrationBuilder.DropTable(
                name: "print_order_status_reason");

            migrationBuilder.DropTable(
                name: "printer");

            migrationBuilder.DropTable(
                name: "print_material");

            migrationBuilder.DropTable(
                name: "request");

            migrationBuilder.DropTable(
                name: "printer_model");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "request_status");

            migrationBuilder.DropTable(
                name: "request_status_reason");

            migrationBuilder.DropTable(
                name: "request_type");

            migrationBuilder.DropTable(
                name: "user_status");
        }
    }
}
