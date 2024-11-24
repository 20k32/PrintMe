﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PrintMe.Server.Persistence;

#nullable disable

namespace PrintMe.Server.Migrations
{
    [DbContext(typeof(PrintMeDbContext))]
    partial class PrintMeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Favourite", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<int>("PrinterId")
                        .HasColumnType("integer")
                        .HasColumnName("printer_id");

                    b.HasKey("UserId", "PrinterId")
                        .HasName("favourites_pkey");

                    b.HasIndex(new[] { "PrinterId" }, "idx_favourites_printer_id");

                    b.HasIndex(new[] { "UserId" }, "idx_favourites_user_id");

                    b.ToTable("favourites", (string)null);
                });

            modelBuilder.Entity("PrintMaterial", b =>
                {
                    b.Property<int>("PrinterId")
                        .HasColumnType("integer")
                        .HasColumnName("printer_id");

                    b.Property<int>("MaterialId")
                        .HasColumnType("integer")
                        .HasColumnName("material_id");

                    b.HasKey("PrinterId", "MaterialId")
                        .HasName("print_materials_pkey");

                    b.HasIndex("MaterialId");

                    b.ToTable("print_materials", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Chat", b =>
                {
                    b.Property<int>("ChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("chat_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ChatId"));

                    b.Property<bool?>("IsArchived")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_archived");

                    b.Property<int?>("User1Id")
                        .HasColumnType("integer")
                        .HasColumnName("user1_id");

                    b.Property<int?>("User2Id")
                        .HasColumnType("integer")
                        .HasColumnName("user2_id");

                    b.HasKey("ChatId")
                        .HasName("chat_pkey");

                    b.HasIndex(new[] { "User1Id" }, "idx_chat_user1_id");

                    b.HasIndex(new[] { "User2Id" }, "idx_chat_user2_id");

                    b.ToTable("chat", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Message", b =>
                {
                    b.Property<int>("ChatId")
                        .HasColumnType("integer")
                        .HasColumnName("chat_id");

                    b.Property<DateTime>("SendTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("send_time")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int?>("SenderId")
                        .HasColumnType("integer")
                        .HasColumnName("sender_id");

                    b.Property<string>("Text")
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.HasKey("ChatId", "SendTime")
                        .HasName("message_pkey");

                    b.HasIndex("SenderId");

                    b.HasIndex(new[] { "ChatId" }, "idx_message_chat_id");

                    b.HasIndex(new[] { "SendTime" }, "idx_message_send_time");

                    b.ToTable("message", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.PrintMaterial", b =>
                {
                    b.Property<int>("PrintMaterialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PrintMaterialId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("PrintMaterialId")
                        .HasName("print_material_id");

                    b.ToTable("print_material", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.PrintOrder", b =>
                {
                    b.Property<int>("PrintOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("print_order_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PrintOrderId"));

                    b.Property<DateOnly>("DueDate")
                        .HasColumnType("date")
                        .HasColumnName("due_date");

                    b.Property<string>("ItemDescription")
                        .HasColumnType("text")
                        .HasColumnName("item_description");

                    b.Property<string>("ItemLink")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("item_link");

                    b.Property<int?>("ItemMaterialId")
                        .HasColumnType("integer")
                        .HasColumnName("item_material_id");

                    b.Property<int?>("ItemQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("item_quantity");

                    b.Property<DateOnly>("OrderDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("order_date")
                        .HasDefaultValueSql("CURRENT_DATE");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("price");

                    b.Property<int?>("PrintOrderStatusId")
                        .HasColumnType("integer")
                        .HasColumnName("print_order_status_id");

                    b.Property<int?>("PrintOrderStatusReasonId")
                        .HasColumnType("integer")
                        .HasColumnName("print_order_status_reason_id");

                    b.Property<int?>("PrinterId")
                        .HasColumnType("integer")
                        .HasColumnName("printer_id");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("PrintOrderId")
                        .HasName("print_order_pkey");

                    b.HasIndex("ItemMaterialId");

                    b.HasIndex("PrintOrderStatusId");

                    b.HasIndex("PrintOrderStatusReasonId");

                    b.HasIndex(new[] { "OrderDate" }, "idx_print_order_order_date");

                    b.HasIndex(new[] { "PrinterId" }, "idx_print_order_printer_id");

                    b.HasIndex(new[] { "UserId" }, "idx_print_order_user_id");

                    b.ToTable("print_order", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.PrintOrderStatus", b =>
                {
                    b.Property<int>("PrintOrderStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("print_order_status_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PrintOrderStatusId"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("PrintOrderStatusId")
                        .HasName("print_order_status_pkey");

                    b.ToTable("print_order_status", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.PrintOrderStatusReason", b =>
                {
                    b.Property<int>("PrintOrderStatusReasonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("print_order_status_reason_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PrintOrderStatusReasonId"));

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.HasKey("PrintOrderStatusReasonId")
                        .HasName("print_order_status_reason_pkey");

                    b.ToTable("print_order_status_reason", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Printer", b =>
                {
                    b.Property<int>("PrinterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("printer_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PrinterId"));

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<double>("LocationX")
                        .HasColumnType("double precision")
                        .HasColumnName("location_x");

                    b.Property<double>("LocationY")
                        .HasColumnType("double precision")
                        .HasColumnName("location_y");

                    b.Property<double>("MaxModelHeight")
                        .HasColumnType("double precision")
                        .HasColumnName("max_model_height");

                    b.Property<double>("MaxModelWidth")
                        .HasColumnType("double precision")
                        .HasColumnName("max_model_width");

                    b.Property<double>("MinModelHeight")
                        .HasColumnType("double precision")
                        .HasColumnName("min_model_height");

                    b.Property<double>("MinModelWidth")
                        .HasColumnType("double precision")
                        .HasColumnName("min_model_width");

                    b.Property<int?>("PrinterModelId")
                        .HasColumnType("integer")
                        .HasColumnName("printer_model_id");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("PrinterId")
                        .HasName("printer_pkey");

                    b.HasIndex(new[] { "PrinterModelId" }, "idx_printer_printer_model_id");

                    b.HasIndex(new[] { "UserId" }, "idx_printer_user_id");

                    b.ToTable("printer", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.PrinterModel", b =>
                {
                    b.Property<int>("PrinterModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("printer_model_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PrinterModelId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("PrinterModelId")
                        .HasName("printer_model_pkey");

                    b.ToTable("printer_model", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Rating", b =>
                {
                    b.Property<int>("RatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("rating_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RatingId"));

                    b.Property<int?>("Reviewer")
                        .HasColumnType("integer")
                        .HasColumnName("reviewer");

                    b.Property<int?>("Target")
                        .HasColumnType("integer")
                        .HasColumnName("target");

                    b.Property<int?>("Value")
                        .HasColumnType("integer")
                        .HasColumnName("value");

                    b.HasKey("RatingId")
                        .HasName("rating_pkey");

                    b.HasIndex(new[] { "Reviewer" }, "idx_rating_reviewer");

                    b.HasIndex(new[] { "Target" }, "idx_rating_target");

                    b.ToTable("rating", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Request", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("request_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RequestId"));

                    b.Property<int?>("DeleteUserId")
                        .HasColumnType("integer")
                        .HasColumnName("delete_user_id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<double?>("LocationX")
                        .HasColumnType("double precision")
                        .HasColumnName("location_x");

                    b.Property<double?>("LocationY")
                        .HasColumnType("double precision")
                        .HasColumnName("location_y");

                    b.Property<double?>("MaxModelHeight")
                        .HasColumnType("double precision")
                        .HasColumnName("max_model_height");

                    b.Property<double?>("MaxModelWidth")
                        .HasColumnType("double precision")
                        .HasColumnName("max_model_width");

                    b.Property<double?>("MinModelHeight")
                        .HasColumnType("double precision")
                        .HasColumnName("min_model_height");

                    b.Property<double?>("MinModelWidth")
                        .HasColumnType("double precision")
                        .HasColumnName("min_model_width");

                    b.Property<int?>("ModelId")
                        .HasColumnType("integer")
                        .HasColumnName("model_id");

                    b.Property<int?>("ReportedUserId")
                        .HasColumnType("integer")
                        .HasColumnName("reported_user_id");

                    b.Property<int>("RequestStatusId")
                        .HasColumnType("integer")
                        .HasColumnName("request_status_id");

                    b.Property<int?>("RequestStatusReasonId")
                        .HasColumnType("integer")
                        .HasColumnName("request_status_reason_id");

                    b.Property<int>("RequestTypeId")
                        .HasColumnType("integer")
                        .HasColumnName("request_type_id");

                    b.Property<int>("UserSenderId")
                        .HasColumnType("integer")
                        .HasColumnName("user_sender_id");

                    b.Property<string>("UserTextData")
                        .HasColumnType("text")
                        .HasColumnName("user_text_data");

                    b.HasKey("RequestId")
                        .HasName("request_pkey");

                    b.HasIndex("DeleteUserId");

                    b.HasIndex("ReportedUserId");

                    b.HasIndex("RequestStatusReasonId");

                    b.HasIndex(new[] { "RequestStatusId" }, "idx_request_request_status_id");

                    b.HasIndex(new[] { "RequestTypeId" }, "idx_request_request_type_id");

                    b.HasIndex(new[] { "UserSenderId" }, "idx_request_user_sender_id");

                    b.ToTable("request", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.RequestStatus", b =>
                {
                    b.Property<int>("RequestStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("request_status_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RequestStatusId"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("RequestStatusId")
                        .HasName("request_status_pkey");

                    b.ToTable("request_status", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.RequestStatusReason", b =>
                {
                    b.Property<int>("RequestStatusReasonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("request_status_reason_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RequestStatusReasonId"));

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.HasKey("RequestStatusReasonId")
                        .HasName("request_status_reason_pkey");

                    b.ToTable("request_status_reason", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.RequestType", b =>
                {
                    b.Property<int>("RequestTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("request_type_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RequestTypeId"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("RequestTypeId")
                        .HasName("request_type_pkey");

                    b.ToTable("request_type", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("PasswordSalt")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("phone_number");

                    b.Property<bool?>("ShouldHidePhoneNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("should_hide_phone_number");

                    b.Property<int?>("UserStatusId")
                        .HasColumnType("integer")
                        .HasColumnName("user_status_id");

                    b.HasKey("UserId")
                        .HasName("user_pkey");

                    b.HasIndex(new[] { "Email" }, "idx_user_email");

                    b.HasIndex(new[] { "PhoneNumber" }, "idx_user_phone_number");

                    b.HasIndex(new[] { "UserStatusId" }, "idx_user_status_id");

                    b.HasIndex(new[] { "Email" }, "user_email_key")
                        .IsUnique();

                    b.HasIndex(new[] { "PhoneNumber" }, "user_phone_number_key")
                        .IsUnique();

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.UserStatus", b =>
                {
                    b.Property<int>("UserStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_status_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserStatusId"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("UserStatusId")
                        .HasName("user_status_pkey");

                    b.ToTable("user_status", (string)null);
                });

            modelBuilder.Entity("RequestPrintMaterial", b =>
                {
                    b.Property<int>("RequestId")
                        .HasColumnType("integer")
                        .HasColumnName("request_id");

                    b.Property<int>("PrintMaterialId")
                        .HasColumnType("integer")
                        .HasColumnName("print_material_id");

                    b.HasKey("RequestId", "PrintMaterialId")
                        .HasName("request_print_materials_pkey");

                    b.HasIndex(new[] { "PrintMaterialId" }, "idx_request_print_materials_material_id");

                    b.HasIndex(new[] { "RequestId" }, "idx_request_print_materials_request_id");

                    b.ToTable("request_print_materials", (string)null);
                });

            modelBuilder.Entity("Favourite", b =>
                {
                    b.HasOne("PrintMe.Server.Persistence.Models.Printer", null)
                        .WithMany()
                        .HasForeignKey("PrinterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("favourites_printer_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("favourites_user_id_fkey");
                });

            modelBuilder.Entity("PrintMaterial", b =>
                {
                    b.HasOne("PrintMe.Server.Persistence.Models.PrintMaterial", null)
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("print_materials_material_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.Printer", null)
                        .WithMany()
                        .HasForeignKey("PrinterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("print_materials_printer_id_fkey");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Chat", b =>
                {
                    b.HasOne("PrintMe.Server.Persistence.Models.User", "User1")
                        .WithMany("ChatUser1s")
                        .HasForeignKey("User1Id")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("chat_user1_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.User", "User2")
                        .WithMany("ChatUser2s")
                        .HasForeignKey("User2Id")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("chat_user2_id_fkey");

                    b.Navigation("User1");

                    b.Navigation("User2");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Message", b =>
                {
                    b.HasOne("PrintMe.Server.Persistence.Models.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("message_chat_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.User", "Sender")
                        .WithMany("Messages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("message_sender_id_fkey");

                    b.Navigation("Chat");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.PrintOrder", b =>
                {
                    b.HasOne("PrintMe.Server.Persistence.Models.PrintMaterial", "ItemMaterial")
                        .WithMany("PrintOrders")
                        .HasForeignKey("ItemMaterialId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("print_order_item_material_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.PrintOrderStatus", "PrintOrderStatus")
                        .WithMany("PrintOrders")
                        .HasForeignKey("PrintOrderStatusId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("print_order_print_order_status_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.PrintOrderStatusReason", "PrintOrderStatusReason")
                        .WithMany("PrintOrders")
                        .HasForeignKey("PrintOrderStatusReasonId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("print_order_print_order_status_reason_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.Printer", "Printer")
                        .WithMany("PrintOrders")
                        .HasForeignKey("PrinterId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("print_order_printer_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.User", "User")
                        .WithMany("PrintOrders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("print_order_user_id_fkey");

                    b.Navigation("ItemMaterial");

                    b.Navigation("PrintOrderStatus");

                    b.Navigation("PrintOrderStatusReason");

                    b.Navigation("Printer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Printer", b =>
                {
                    b.HasOne("PrintMe.Server.Persistence.Models.PrinterModel", "PrinterModel")
                        .WithMany("Printers")
                        .HasForeignKey("PrinterModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("printer_printer_model_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.User", "User")
                        .WithMany("PrintersNavigation")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("printer_user_id_fkey");

                    b.Navigation("PrinterModel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Rating", b =>
                {
                    b.HasOne("PrintMe.Server.Persistence.Models.User", "ReviewerNavigation")
                        .WithMany("RatingReviewerNavigations")
                        .HasForeignKey("Reviewer")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("rating_reviewer_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.User", "TargetNavigation")
                        .WithMany("RatingTargetNavigations")
                        .HasForeignKey("Target")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("rating_target_fkey");

                    b.Navigation("ReviewerNavigation");

                    b.Navigation("TargetNavigation");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Request", b =>
                {
                    b.HasOne("PrintMe.Server.Persistence.Models.User", "DeleteUser")
                        .WithMany("RequestDeleteUsers")
                        .HasForeignKey("DeleteUserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("request_delete_user_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.User", "ReportedUser")
                        .WithMany("RequestReportedUsers")
                        .HasForeignKey("ReportedUserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("request_reported_user_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.RequestStatus", "RequestStatus")
                        .WithMany("Requests")
                        .HasForeignKey("RequestStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("request_request_status_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.RequestStatusReason", "RequestStatusReason")
                        .WithMany("Requests")
                        .HasForeignKey("RequestStatusReasonId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("request_request_status_reason_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.RequestType", "RequestType")
                        .WithMany("Requests")
                        .HasForeignKey("RequestTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("request_request_type_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.User", "UserSender")
                        .WithMany("RequestUserSenders")
                        .HasForeignKey("UserSenderId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("request_user_sender_id_fkey");

                    b.Navigation("DeleteUser");

                    b.Navigation("ReportedUser");

                    b.Navigation("RequestStatus");

                    b.Navigation("RequestStatusReason");

                    b.Navigation("RequestType");

                    b.Navigation("UserSender");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.User", b =>
                {
                    b.HasOne("PrintMe.Server.Persistence.Models.UserStatus", "UserStatus")
                        .WithMany("Users")
                        .HasForeignKey("UserStatusId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("user_user_status_id_fkey");

                    b.Navigation("UserStatus");
                });

            modelBuilder.Entity("RequestPrintMaterial", b =>
                {
                    b.HasOne("PrintMe.Server.Persistence.Models.PrintMaterial", null)
                        .WithMany()
                        .HasForeignKey("PrintMaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("request_print_materials_print_material_id_fkey");

                    b.HasOne("PrintMe.Server.Persistence.Models.Request", null)
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("request_print_materials_request_id_fkey");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Chat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.PrintMaterial", b =>
                {
                    b.Navigation("PrintOrders");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.PrintOrderStatus", b =>
                {
                    b.Navigation("PrintOrders");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.PrintOrderStatusReason", b =>
                {
                    b.Navigation("PrintOrders");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.Printer", b =>
                {
                    b.Navigation("PrintOrders");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.PrinterModel", b =>
                {
                    b.Navigation("Printers");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.RequestStatus", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.RequestStatusReason", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.RequestType", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.User", b =>
                {
                    b.Navigation("ChatUser1s");

                    b.Navigation("ChatUser2s");

                    b.Navigation("Messages");

                    b.Navigation("PrintOrders");

                    b.Navigation("PrintersNavigation");

                    b.Navigation("RatingReviewerNavigations");

                    b.Navigation("RatingTargetNavigations");

                    b.Navigation("RequestDeleteUsers");

                    b.Navigation("RequestReportedUsers");

                    b.Navigation("RequestUserSenders");
                });

            modelBuilder.Entity("PrintMe.Server.Persistence.Models.UserStatus", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
