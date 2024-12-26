using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AlmazAucceon_3_.Model2;

public partial class AuctionContext : DbContext
{
    public AuctionContext()
    {
    }

    public AuctionContext(DbContextOptions<AuctionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Spisactovar> Spisactovars { get; set; }

    public virtual DbSet<Staffe> Staffes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=auction", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategories).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.Property(e => e.IdCategories).HasColumnName("id_categories");
            entity.Property(e => e.Title)
                .HasMaxLength(45)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.ToTable("items");

            entity.HasIndex(e => e.CategotiaId, "id_idx");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.CategotiaId).HasColumnName("categotia_id");
            entity.Property(e => e.CurrentPrice).HasColumnName("current_price");
            entity.Property(e => e.ImageItems).HasColumnName("image_items");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_deleted");
            entity.Property(e => e.ItemName)
                .HasMaxLength(25)
                .HasColumnName("item_name");

            entity.HasOne(d => d.Categotia).WithMany(p => p.Items)
                .HasForeignKey(d => d.CategotiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(25)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Spisactovar>(entity =>
        {
            entity.HasKey(e => e.IdSpisacTovars).HasName("PRIMARY");

            entity.ToTable("spisactovars");

            entity.HasIndex(e => e.IdProdyct, "ItemsHol_idx");

            entity.HasIndex(e => e.IdUser, "userID_idx");

            entity.Property(e => e.IdSpisacTovars).HasColumnName("idSpisacTovars");
            entity.Property(e => e.IdUser).HasColumnName("idUser");

            entity.HasOne(d => d.IdProdyctNavigation).WithMany(p => p.Spisactovars)
                .HasForeignKey(d => d.IdProdyct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ItemsHol");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Spisactovars)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userID");
        });

        modelBuilder.Entity<Staffe>(entity =>
        {
            entity.HasKey(e => e.IdStaffes).HasName("PRIMARY");

            entity.ToTable("staffes");

            entity.HasIndex(e => e.IdStaffRole, "id_staff_role");

            entity.Property(e => e.IdStaffes).HasColumnName("id_staffes");
            entity.Property(e => e.Data)
                .HasMaxLength(6)
                .HasColumnName("data");
            entity.Property(e => e.Email)
                .HasMaxLength(25)
                .HasColumnName("email");
            entity.Property(e => e.IdStaffRole).HasColumnName("id_staff_role");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.StaffLastname)
                .HasMaxLength(15)
                .HasColumnName("staff_lastname");
            entity.Property(e => e.StaffLogin)
                .HasMaxLength(25)
                .HasColumnName("staff_login");
            entity.Property(e => e.StaffName)
                .HasMaxLength(15)
                .HasColumnName("staff_name");
            entity.Property(e => e.StaffPatronymic)
                .HasMaxLength(15)
                .HasColumnName("staff_patronymic");
            entity.Property(e => e.StaffPsswords)
                .HasMaxLength(25)
                .HasColumnName("staff_psswords");

            entity.HasOne(d => d.IdStaffRoleNavigation).WithMany(p => p.Staffes)
                .HasForeignKey(d => d.IdStaffRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("staffes_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.IdRole, "idRole");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.DataUser)
                .HasMaxLength(6)
                .HasColumnName("data_user");
            entity.Property(e => e.Email)
                .HasMaxLength(25)
                .HasColumnName("email");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Lastname)
                .HasMaxLength(15)
                .HasColumnName("lastname");
            entity.Property(e => e.Login)
                .HasMaxLength(25)
                .HasColumnName("login");
            entity.Property(e => e.Money)
                .HasMaxLength(20)
                .HasColumnName("money");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(15)
                .HasColumnName("patronymic");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.Psswords)
                .HasMaxLength(25)
                .HasColumnName("psswords");
            entity.Property(e => e.UserName)
                .HasMaxLength(15)
                .HasColumnName("user_name");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("idRole");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
