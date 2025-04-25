using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Models;

public partial class EmanagerContext : DbContext
{
    public EmanagerContext()
    {
    }

    public EmanagerContext(DbContextOptions<EmanagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<StClass> StClasses { get; set; }

    public virtual DbSet<StStatus> StStatuses { get; set; }

    public virtual DbSet<TAuth> TAuths { get; set; }

    public virtual DbSet<TEmployee> TEmployees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-SCVCKCQ\\SQLEXPRESS;Initial Catalog=emanager;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("st_class_pk");

            entity.ToTable("st_class");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<StStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("st_status_pk");

            entity.ToTable("st_status");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TAuth>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("t_auth_pk");

            entity.ToTable("t_auth");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("employee_id");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<TEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("t_employee_pk");

            entity.ToTable("t_employee");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Age)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("age");
            entity.Property(e => e.Class)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("class");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Salary)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("salary");
            entity.Property(e => e.Status)
                .HasColumnType("numeric(38, 0)")
                .HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
