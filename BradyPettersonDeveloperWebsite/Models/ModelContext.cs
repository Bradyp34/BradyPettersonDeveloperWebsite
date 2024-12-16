using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<FeatureTask> FeatureTasks { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectUser> ProjectUsers { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskUser> TaskUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseJet("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='C:\\Users\\Brady\\OneDrive - Dakota State University\\Documents\\personal\\BradyPettersonWebsiteDatabase.accdb';");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PrimaryKey");

            entity.ToTable("Feature");

            entity.Property(e => e.Id)
                .HasColumnType("counter")
                .HasColumnName("ID");
            entity.Property(e => e.FeatureName).HasMaxLength(255);
            entity.Property(e => e.MoScoWclassification)
                .HasDefaultValue(0)
                .HasColumnName("MoSCoWClassification");
        });

        modelBuilder.Entity<FeatureTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PrimaryKey");

            entity.ToTable("FeatureTask");

            entity.HasIndex(e => e.FeatureId, "FeatureID");

            entity.HasIndex(e => e.TaskId, "TaskID");

            entity.Property(e => e.Id)
                .HasColumnType("counter")
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FeatureId)
                .HasDefaultValue(0)
                .HasColumnName("FeatureID");
            entity.Property(e => e.TaskId)
                .HasDefaultValue(0)
                .HasColumnName("TaskID");

            entity.HasOne(d => d.Feature).WithMany(p => p.FeatureTasks)
                .HasForeignKey(d => d.FeatureId)
                .HasConstraintName("FeatureFeatureTask");

            entity.HasOne(d => d.Task).WithMany(p => p.FeatureTasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("TaskFeatureTask");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PrimaryKey");

            entity.ToTable("Project");

            entity.Property(e => e.Id)
                .HasColumnType("counter")
                .HasColumnName("ID");
            entity.Property(e => e.ProjectName).HasMaxLength(255);
        });

        modelBuilder.Entity<ProjectUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PrimaryKey");

            entity.ToTable("ProjectUser");

            entity.HasIndex(e => e.ProjectId, "ProjectID");

            entity.HasIndex(e => e.UserId, "UserID");

            entity.Property(e => e.Id)
                .HasColumnType("counter")
                .HasColumnName("ID");
            entity.Property(e => e.ProjectId)
                .HasDefaultValue(0)
                .HasColumnName("ProjectID");
            entity.Property(e => e.UserId)
                .HasDefaultValue(0)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectUsers)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("ProjectProjectUser");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PrimaryKey");

            entity.ToTable("Task");

            entity.HasIndex(e => e.AssigneeId, "AssigneeID");

            entity.HasIndex(e => e.ProjectId, "ProjectID");

            entity.Property(e => e.Id)
                .HasColumnType("counter")
                .HasColumnName("ID");
            entity.Property(e => e.AssigneeId)
                .HasDefaultValue(0)
                .HasColumnName("AssigneeID");
            entity.Property(e => e.Completed)
                .HasDefaultValueSql("No")
                .HasColumnType("bit");
            entity.Property(e => e.ProjectId)
                .HasDefaultValue(0)
                .HasColumnName("ProjectID");
            entity.Property(e => e.Stage).HasDefaultValue(0);
            entity.Property(e => e.TaskName).HasMaxLength(255);

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("ProjectTask");
        });

        modelBuilder.Entity<TaskUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PrimaryKey");

            entity.ToTable("TaskUser");

            entity.HasIndex(e => e.TaskId, "TaskID");

            entity.HasIndex(e => e.UserId, "UserID");

            entity.Property(e => e.Id)
                .HasColumnType("counter")
                .HasColumnName("ID");
            entity.Property(e => e.TaskId)
                .HasDefaultValue(0)
                .HasColumnName("TaskID");
            entity.Property(e => e.UserId)
                .HasDefaultValue(0)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskUsers)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("TaskTaskUser");

            entity.HasOne(d => d.User).WithMany(p => p.TaskUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserTaskUser");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PrimaryKey");

            entity.ToTable("User");

            entity.Property(e => e.Id)
                .HasColumnType("counter")
                .HasColumnName("ID");
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.JobTitle)
                .HasMaxLength(255)
                .HasColumnName("Job Title");
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
