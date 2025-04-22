using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LMS.Models.LMSModels
{
    public partial class LMSContext : DbContext
    {
        public LMSContext()
        {
        }

        public LMSContext(DbContextOptions<LMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrator> Administrators { get; set; } = null!;
        public virtual DbSet<Assignment> Assignments { get; set; } = null!;
        public virtual DbSet<AssignmentCategory> AssignmentCategories { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; } = null!;
        public virtual DbSet<Enrolled> Enrolleds { get; set; } = null!;
        public virtual DbSet<Professor> Professors { get; set; } = null!;
        public virtual DbSet<Sshkey> Sshkeys { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Submission> Submissions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("name=LMS:LMSConnectionString", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.8-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("latin1_swedish_ci")
                .HasCharSet("latin1");

            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.Property(e => e.UserId)
                    .HasMaxLength(8)
                    .HasColumnName("UserID");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(45);

                entity.Property(e => e.LastName).HasMaxLength(45);
            });

            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.Category }, "Assignment_UQ")
                    .IsUnique();

                entity.HasIndex(e => e.Category, "Category_FK_idx");

                entity.Property(e => e.AssignmentId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("AssignmentID");

                entity.Property(e => e.Category).HasColumnType("int(11)");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.Instructions).HasMaxLength(8192);

                entity.Property(e => e.MaxValue).HasColumnType("int(11)");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Category_FK");
            });

            modelBuilder.Entity<AssignmentCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.Class, e.Name }, "Category_UQ")
                    .IsUnique();

                entity.HasIndex(e => e.Class, "Class_FK_idx");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("CategoryID");

                entity.Property(e => e.Class).HasColumnType("int(11)");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Weight).HasColumnType("int(3)");

                entity.HasOne(d => d.ClassNavigation)
                    .WithMany(p => p.AssignmentCategories)
                    .HasForeignKey(d => d.Class)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Class_FK");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasIndex(e => new { e.Season, e.Course, e.Year }, "Classes_UQ")
                    .IsUnique();

                entity.HasIndex(e => e.Professor, "CourseProf_FK_idx");

                entity.HasIndex(e => e.Course, "Courses_FK_idx");

                entity.Property(e => e.ClassId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("ClassID");

                entity.Property(e => e.Course).HasColumnType("int(11)");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Location).HasMaxLength(100);

                entity.Property(e => e.Professor).HasMaxLength(8);

                entity.Property(e => e.Season).HasMaxLength(10);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Year).HasColumnType("int(11)");

                entity.HasOne(d => d.CourseNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.Course)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Courses_FK");

                entity.HasOne(d => d.ProfessorNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.Professor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CourseProf_FK");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.Departments, "Department_FK_idx");

                entity.HasIndex(e => new { e.Name, e.Departments }, "name_dept_uq")
                    .IsUnique();

                entity.Property(e => e.CourseId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("CourseID");

                entity.Property(e => e.Departments).HasMaxLength(4);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Number).HasColumnType("int(4)");

                entity.HasOne(d => d.DepartmentsNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.Departments)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Dep_FK");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Abbreviation)
                    .HasName("PRIMARY");

                entity.Property(e => e.Abbreviation).HasMaxLength(4);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<EfmigrationsHistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId)
                    .HasName("PRIMARY");

                entity.ToTable("__EFMigrationsHistory");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ProductVersion).HasMaxLength(32);
            });

            modelBuilder.Entity<Enrolled>(entity =>
            {
                entity.HasKey(e => new { e.Student, e.Class })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("Enrolled");

                entity.HasIndex(e => e.Class, "ClassEnrolled_FK_idx");

                entity.Property(e => e.Student).HasMaxLength(8);

                entity.Property(e => e.Class).HasColumnType("int(11)");

                entity.Property(e => e.Grade).HasMaxLength(2);

                entity.HasOne(d => d.ClassNavigation)
                    .WithMany(p => p.Enrolleds)
                    .HasForeignKey(d => d.Class)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ClassEnrolled_FK");

                entity.HasOne(d => d.StudentNavigation)
                    .WithMany(p => p.Enrolleds)
                    .HasForeignKey(d => d.Student)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StudentEnrolled_FK");
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Department, "WorkFor_FK_idx");

                entity.Property(e => e.UserId)
                    .HasMaxLength(8)
                    .HasColumnName("UserID");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Department).HasMaxLength(4);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.HasOne(d => d.DepartmentNavigation)
                    .WithMany(p => p.Professors)
                    .HasForeignKey(d => d.Department)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkFor_FK");
            });

            modelBuilder.Entity<Sshkey>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sshkey");

                entity.Property(e => e.Sshkey1)
                    .HasColumnType("text")
                    .HasColumnName("sshkey");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Major, "Major_FK_idx");

                entity.Property(e => e.UserId)
                    .HasMaxLength(8)
                    .HasColumnName("UserID");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Major).HasMaxLength(4);

                entity.HasOne(d => d.MajorNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.Major)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Major_FK");
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.HasKey(e => new { e.Student, e.Assignment })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasIndex(e => e.Assignment, "AssignmentSub_FK_idx");

                entity.Property(e => e.Student).HasMaxLength(8);

                entity.Property(e => e.Assignment).HasColumnType("int(11)");

                entity.Property(e => e.Contents).HasMaxLength(8192);

                entity.Property(e => e.Score).HasColumnType("int(11)");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.AssignmentNavigation)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.Assignment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AssignmentSub_FK");

                entity.HasOne(d => d.StudentNavigation)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.Student)
                    .HasConstraintName("StudentSub_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
