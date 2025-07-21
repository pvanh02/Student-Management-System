using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Finally.Models;

public partial class FinallyContext : DbContext
{
    public FinallyContext()
    {
    }

    public FinallyContext(DbContextOptions<FinallyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Server=localhost;database=Finally;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3214EC275B482C4D");

            entity.ToTable("Admin");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Admins)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Admin__Role_ID__398D8EEE");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Classes__3214EC272CC3F30F");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC2728FB270D");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Grades__3214EC277AB8CD1F");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClassId).HasColumnName("Class_ID");
            entity.Property(e => e.Grade1).HasColumnName("grade");
            entity.Property(e => e.StudentId).HasColumnName("Student_ID");
            entity.Property(e => e.TeacherId).HasColumnName("Teacher_ID");

            entity.HasOne(d => d.Class).WithMany(p => p.Grades)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Grades__Class_ID__4CA06362");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Grades__Student___4BAC3F29");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Grades)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__Grades__Teacher___4D94879B");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC27FEC01903");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC27D2BE9918");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClassId).HasColumnName("Class_ID");
            entity.Property(e => e.DayOfWeeks).HasMaxLength(50);
            entity.Property(e => e.Slot).HasColumnName("slot");
            entity.Property(e => e.TeacherId).HasColumnName("Teacher_ID");

            entity.HasOne(d => d.Class).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Schedules__Class__47DBAE45");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__Schedules__Teach__48CFD27E");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC279E2F1A35");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.ClassId).HasColumnName("Class_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Students__Class___44FF419A");

            entity.HasOne(d => d.Role).WithMany(p => p.Students)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Students__Role_I__440B1D61");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teachers__3214EC27FB9EE92A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.DepartmentId).HasColumnName("Department_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.Subject)
                .HasMaxLength(50)
                .HasColumnName("subject");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Department).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Teachers__Depart__3F466844");

            entity.HasOne(d => d.Role).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Teachers__Role_I__3E52440B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
