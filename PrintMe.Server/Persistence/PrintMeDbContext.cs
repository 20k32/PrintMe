using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;
using PrintMe.Server.Constants;

namespace PrintMe.Server.Persistence;

public partial class PrintMeDbContext : DbContext
{
    public PrintMeDbContext()
    {
    }

    public PrintMeDbContext(DbContextOptions<PrintMeDbContext> options)
        : base(options)
    {
#if DEBUG
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
#endif
        
    }

    public async Task LoadTestDataAsync()
    {
        // await UserRoles.GenerateForUserRolesAsync();
        // await UserStatuses.GenerateForUserStatusesAsync();
        await Users.GenerateForUsersAsync(15);
        await Printers.GenerateForPrintersAsync(PrintMaterials1, PrinterModels, 15, 4, 10);
        
        await SaveChangesAsync();
    }
    
    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<PrintMaterial> PrintMaterials1 { get; set; }

    public virtual DbSet<PrintOrder> PrintOrders { get; set; }

    public virtual DbSet<PrintOrderStatus> PrintOrderStatuses { get; set; }

    public virtual DbSet<PrintOrderStatusReason> PrintOrderStatusReasons { get; set; }

    public virtual DbSet<Printer> Printers { get; set; }

    public virtual DbSet<PrinterModel> PrinterModels { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestStatus> RequestStatuses { get; set; }

    public virtual DbSet<RequestStatusReason> RequestStatusReasons { get; set; }

    public virtual DbSet<RequestType> RequestTypes { get; set; }
    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserStatus> UserStatuses { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING_PRINTME_DB"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("chat_pkey");

            entity.ToTable("chat");

            entity.HasIndex(e => e.User1Id, "idx_chat_user1_id");

            entity.HasIndex(e => e.User2Id, "idx_chat_user2_id");

            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.IsArchived)
                .HasDefaultValue(false)
                .HasColumnName("is_archived");
            entity.Property(e => e.User1Id).HasColumnName("user1_id");
            entity.Property(e => e.User2Id).HasColumnName("user2_id");

            entity.HasOne(d => d.User1).WithMany(p => p.ChatUser1s)
                .HasForeignKey(d => d.User1Id)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("chat_user1_id_fkey");

            entity.HasOne(d => d.User2).WithMany(p => p.ChatUser2s)
                .HasForeignKey(d => d.User2Id)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("chat_user2_id_fkey");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => new { e.ChatId, e.SendTime }).HasName("message_pkey");

            entity.ToTable("message");

            entity.HasIndex(e => e.ChatId, "idx_message_chat_id");

            entity.HasIndex(e => e.SendTime, "idx_message_send_time");

            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.SendTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("send_time");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.Text).HasColumnName("text");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("message_chat_id_fkey");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("message_sender_id_fkey");
        });

        modelBuilder.Entity<PrintMaterial>(entity =>
        {
            entity.HasKey(e => e.PrintMaterialId).HasName("print_material_id");

            entity.ToTable("print_material");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name");

            entity.HasIndex(e => e.Name, "idx_print_material_name")
                .IsUnique();
            
        });

        modelBuilder.Entity<PrintOrder>(entity =>
        {
            entity.HasKey(e => e.PrintOrderId).HasName("print_order_pkey");

            entity.ToTable("print_order");

            entity.HasIndex(e => e.OrderDate, "idx_print_order_order_date");

            entity.HasIndex(e => e.PrinterId, "idx_print_order_printer_id");

            entity.HasIndex(e => e.UserId, "idx_print_order_user_id");

            entity.Property(e => e.PrintOrderId).HasColumnName("print_order_id");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.ItemDescription).HasColumnName("item_description");
            entity.Property(e => e.ItemLink)
                .IsRequired()
                .HasColumnName("item_link");
            entity.Property(e => e.ItemMaterialId).HasColumnName("item_material_id");
            entity.Property(e => e.ItemQuantity).HasColumnName("item_quantity");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("order_date");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.PrintOrderStatusId).HasColumnName("print_order_status_id");
            entity.Property(e => e.PrintOrderStatusReasonId).HasColumnName("print_order_status_reason_id");
            entity.Property(e => e.PrinterId).HasColumnName("printer_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ItemMaterial).WithMany(p => p.PrintOrders)
                .HasForeignKey(d => d.ItemMaterialId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("print_order_item_material_id_fkey");

            entity.HasOne(d => d.PrintOrderStatus).WithMany(p => p.PrintOrders)
                .HasForeignKey(d => d.PrintOrderStatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("print_order_print_order_status_id_fkey");

            entity.HasOne(d => d.PrintOrderStatusReason).WithMany(p => p.PrintOrders)
                .HasForeignKey(d => d.PrintOrderStatusReasonId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("print_order_print_order_status_reason_id_fkey");

            entity.HasOne(d => d.Printer).WithMany(p => p.PrintOrders)
                .HasForeignKey(d => d.PrinterId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("print_order_printer_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.PrintOrders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("print_order_user_id_fkey");
        });

        modelBuilder.Entity<PrintOrderStatus>(entity =>
        {
            entity.HasKey(e => e.PrintOrderStatusId).HasName("print_order_status_pkey");

            entity.ToTable("print_order_status");

            entity.Property(e => e.PrintOrderStatusId).HasColumnName("print_order_status_id");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasColumnName("status");

            modelBuilder.Entity<PrintOrderStatus>().HasData(
                DbConstants.PrintOrderStatus.Dictionary.Select(x => 
                    new PrintOrderStatus { PrintOrderStatusId = x.Value, Status = x.Key }));
        });

        modelBuilder.Entity<PrintOrderStatusReason>(entity =>
        {
            entity.HasKey(e => e.PrintOrderStatusReasonId).HasName("print_order_status_reason_pkey");

            entity.ToTable("print_order_status_reason");

            entity.Property(e => e.PrintOrderStatusReasonId).HasColumnName("print_order_status_reason_id");
            entity.Property(e => e.Reason)
                .IsRequired()
                .HasColumnName("reason");

            modelBuilder.Entity<PrintOrderStatusReason>().HasData(
                DbConstants.PrintOrderStatusReason.Dictionary.Select(x => 
                    new PrintOrderStatusReason { PrintOrderStatusReasonId = x.Value, Reason = x.Key }));
        });

        modelBuilder.Entity<Printer>(entity =>
        {
            entity.HasKey(e => e.PrinterId).HasName("printer_pkey");

            entity.ToTable("printer");

            entity.HasIndex(e => e.PrinterModelId, "idx_printer_printer_model_id");

            entity.HasIndex(e => e.UserId, "idx_printer_user_id");

            entity.Property(e => e.PrinterId).HasColumnName("printer_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LocationX).HasColumnName("location_x");
            entity.Property(e => e.LocationY).HasColumnName("location_y");
            entity.Property(e => e.MaxModelHeight).HasColumnName("max_model_height");
            entity.Property(e => e.MaxModelWidth).HasColumnName("max_model_width");
            entity.Property(e => e.MinModelHeight).HasColumnName("min_model_height");
            entity.Property(e => e.MinModelWidth).HasColumnName("min_model_width");
            entity.Property(e => e.PrinterModelId).HasColumnName("printer_model_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.IsDeactivated)
                .HasColumnName("is_deactivated")
                .HasDefaultValue(false);

            entity.HasOne(d => d.PrinterModel).WithMany(p => p.Printers)
                .HasForeignKey(d => d.PrinterModelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("printer_printer_model_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.PrintersNavigation)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("printer_user_id_fkey");

            entity.HasMany(d => d.Materials).WithMany(p => p.Printers)
                .UsingEntity<Dictionary<string, object>>(
                    "PrintMaterial",
                    r => r.HasOne<PrintMaterial>().WithMany()
                        .HasForeignKey("MaterialId")
                        .HasConstraintName("print_materials_material_id_fkey"),
                    l => l.HasOne<Printer>().WithMany()
                        .HasForeignKey("PrinterId")
                        .HasConstraintName("print_materials_printer_id_fkey"),
                    j =>
                    {
                        j.HasKey("PrinterId", "MaterialId").HasName("print_materials_pkey");
                        j.ToTable("print_materials");
                        j.IndexerProperty<int>("PrinterId").HasColumnName("printer_id");
                        j.IndexerProperty<int>("MaterialId").HasColumnName("material_id");
                    });
        });

        modelBuilder.Entity<PrinterModel>(entity =>
        {
            entity.HasKey(e => e.PrinterModelId).HasName("printer_model_pkey");

            entity.ToTable("printer_model");

            entity.Property(e => e.PrinterModelId).HasColumnName("printer_model_id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("rating_pkey");

            entity.ToTable("rating");

            entity.HasIndex(e => e.Reviewer, "idx_rating_reviewer");

            entity.HasIndex(e => e.Target, "idx_rating_target");

            entity.Property(e => e.RatingId).HasColumnName("rating_id");
            entity.Property(e => e.Reviewer).HasColumnName("reviewer");
            entity.Property(e => e.Target).HasColumnName("target");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.ReviewerNavigation).WithMany(p => p.RatingReviewerNavigations)
                .HasForeignKey(d => d.Reviewer)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("rating_reviewer_fkey");

            entity.HasOne(d => d.TargetNavigation).WithMany(p => p.RatingTargetNavigations)
                .HasForeignKey(d => d.Target)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("rating_target_fkey");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("request_pkey");

            entity.ToTable("request");

            entity.HasIndex(e => e.RequestStatusId, "idx_request_request_status_id");

            entity.HasIndex(e => e.RequestTypeId, "idx_request_request_type_id");

            entity.HasIndex(e => e.UserSenderId, "idx_request_user_sender_id");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.DeleteUserId).HasColumnName("delete_user_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LocationX).HasColumnName("location_x");
            entity.Property(e => e.LocationY).HasColumnName("location_y");
            entity.Property(e => e.MaxModelHeight).HasColumnName("max_model_height");
            entity.Property(e => e.MaxModelWidth).HasColumnName("max_model_width");
            entity.Property(e => e.MinModelHeight).HasColumnName("min_model_height");
            entity.Property(e => e.MinModelWidth).HasColumnName("min_model_width");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.ReportedUserId).HasColumnName("reported_user_id");
            entity.Property(e => e.RequestStatusId).HasColumnName("request_status_id");
            entity.Property(e => e.RequestStatusReasonId).HasColumnName("request_status_reason_id");
            entity.Property(e => e.RequestTypeId).HasColumnName("request_type_id");
            entity.Property(e => e.UserSenderId).HasColumnName("user_sender_id");
            entity.Property(e => e.UserTextData).HasColumnName("user_text_data");

            entity.HasOne(d => d.DeleteUser).WithMany(p => p.RequestDeleteUsers)
                .HasForeignKey(d => d.DeleteUserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("request_delete_user_id_fkey");

            entity.HasOne(d => d.ReportedUser).WithMany(p => p.RequestReportedUsers)
                .HasForeignKey(d => d.ReportedUserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("request_reported_user_id_fkey");

            entity.HasOne(d => d.RequestStatus).WithMany(p => p.Requests)
                .HasForeignKey(d => d.RequestStatusId)
                .HasConstraintName("request_request_status_id_fkey");

            entity.HasOne(d => d.RequestStatusReason).WithMany(p => p.Requests)
                .HasForeignKey(d => d.RequestStatusReasonId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("request_request_status_reason_id_fkey");

            entity.HasOne(d => d.RequestType).WithMany(p => p.Requests)
                .HasForeignKey(d => d.RequestTypeId)
                .HasConstraintName("request_request_type_id_fkey");

            entity.HasOne(d => d.UserSender).WithMany(p => p.RequestUserSenders)
                .HasForeignKey(d => d.UserSenderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("request_user_sender_id_fkey");

            entity.HasMany(d => d.PrintMaterials).WithMany(p => p.Requests)
                .UsingEntity<Dictionary<string, object>>(
                    "RequestPrintMaterial",
                    r => r.HasOne<PrintMaterial>().WithMany()
                        .HasForeignKey("PrintMaterialId")
                        .HasConstraintName("request_print_materials_print_material_id_fkey"),
                    l => l.HasOne<Request>().WithMany()
                        .HasForeignKey("RequestId")
                        .HasConstraintName("request_print_materials_request_id_fkey"),
                    j =>
                    {
                        j.HasKey("RequestId", "PrintMaterialId").HasName("request_print_materials_pkey");
                        j.ToTable("request_print_materials");
                        j.HasIndex(new[] { "PrintMaterialId" }, "idx_request_print_materials_material_id");
                        j.HasIndex(new[] { "RequestId" }, "idx_request_print_materials_request_id");
                        j.IndexerProperty<int>("RequestId").HasColumnName("request_id");
                        j.IndexerProperty<int>("PrintMaterialId").HasColumnName("print_material_id");
                    });
        });

        modelBuilder.Entity<RequestStatus>(entity =>
        {
            entity.HasKey(e => e.RequestStatusId).HasName("request_status_pkey");

            entity.ToTable("request_status");

            entity.Property(e => e.RequestStatusId).HasColumnName("request_status_id");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasColumnName("status");

            modelBuilder.Entity<RequestStatus>().HasData(
                DbConstants.RequestStatus.Dictionary.Select(x => 
                    new RequestStatus { RequestStatusId = x.Value, Status = x.Key }));
        });

        modelBuilder.Entity<RequestStatusReason>(entity =>
        {
            entity.HasKey(e => e.RequestStatusReasonId).HasName("request_status_reason_pkey");

            entity.ToTable("request_status_reason");

            entity.Property(e => e.RequestStatusReasonId).HasColumnName("request_status_reason_id");
            entity.Property(e => e.Reason)
                .IsRequired()
                .HasColumnName("reason");

            modelBuilder.Entity<RequestStatusReason>().HasData(
                DbConstants.RequestStatusReason.Dictionary.Select(x => 
                    new RequestStatusReason { RequestStatusReasonId = x.Value, Reason = x.Key }));
        });

        modelBuilder.Entity<RequestType>(entity =>
        {
            entity.HasKey(e => e.RequestTypeId).HasName("request_type_pkey");

            entity.ToTable("request_type");

            entity.Property(e => e.RequestTypeId).HasColumnName("request_type_id");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasColumnName("type");

            modelBuilder.Entity<RequestType>().HasData(
                DbConstants.RequestType.Dictionary.Select(x => 
                    new RequestType { RequestTypeId = x.Value, Type = x.Key }));
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "idx_user_email");

            entity.HasIndex(e => e.PhoneNumber, "idx_user_phone_number");

            entity.HasIndex(e => e.UserStatusId, "idx_user_status_id");

            entity.HasIndex(e => e.Email, "user_email_key").IsUnique();

            // entity.HasIndex(e => e.PhoneNumber, "user_phone_number_key").IsUnique();

            entity.HasIndex(e => e.UserRoleId, "idx_user_role_id");
            
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnName("password");
            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasColumnName("salt");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.ShouldHidePhoneNumber)
                .HasDefaultValue(true)
                .HasColumnName("should_hide_phone_number");
            entity.Property(e => e.UserStatusId).HasColumnName("user_status_id");
            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");


            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("user_user_role_id_fkey");
            
            entity.HasOne(d => d.UserStatus).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserStatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("user_user_status_id_fkey");

            entity.HasMany(d => d.Printers).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Favourite",
                    r => r.HasOne<Printer>().WithMany()
                        .HasForeignKey("PrinterId")
                        .HasConstraintName("favourites_printer_id_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("favourites_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "PrinterId").HasName("favourites_pkey");
                        j.ToTable("favourites");
                        j.HasIndex(new[] { "PrinterId" }, "idx_favourites_printer_id");
                        j.HasIndex(new[] { "UserId" }, "idx_favourites_user_id");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("PrinterId").HasColumnName("printer_id");
                    });
        });

        modelBuilder.Entity<UserStatus>(entity =>
        {
            entity.HasKey(e => e.UserStatusId).HasName("user_status_pkey");

            entity.ToTable("user_status");

            entity.Property(e => e.UserStatusId).HasColumnName("user_status_id");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasColumnName("status");

            modelBuilder.Entity<UserStatus>().HasData(
                DbConstants.UserStatus.Dictionary.Select(x => 
                    new UserStatus { UserStatusId = x.Value, Status = x.Key }));
        });
        
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("user_role_pkey");
            entity.ToTable("user_role");
            entity.Property(e => e.UserRoleName)
                .IsRequired()
                .HasColumnName("user_role_name");

            modelBuilder.Entity<UserRole>().HasData(
                DbConstants.UserRole.Dictionary.Select(x => 
                    new UserRole { UserRoleId = x.Value, UserRoleName = x.Key }));
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
