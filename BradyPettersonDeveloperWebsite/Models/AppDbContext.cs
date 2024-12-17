using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BradyPettersonDeveloperWebsite.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Projecttask> Projecttasks { get; set; }

    public virtual DbSet<Projectuser> Projectusers { get; set; }

    public virtual DbSet<Siteuser> Siteusers { get; set; }

    public virtual DbSet<Taskuser> Taskusers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("feature_pkey");

            entity.ToTable("feature");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Featurename).HasColumnName("featurename");
            entity.Property(e => e.Moscow).HasColumnName("moscow");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("project_pkey");

            entity.ToTable("project");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Projectname).HasColumnName("projectname");
        });

        modelBuilder.Entity<Projecttask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projecttask_pkey");

            entity.ToTable("projecttask");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Assigneeid).HasColumnName("assigneeid");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.Due).HasColumnName("due");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Stage).HasColumnName("stage");
            entity.Property(e => e.Started).HasColumnName("started");
            entity.Property(e => e.Taskname).HasColumnName("taskname");
        });

        modelBuilder.Entity<Projectuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projectuser_pkey");

            entity.ToTable("projectuser");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Siteuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("siteuser_pkey");

            entity.ToTable("siteuser");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fullname).HasColumnName("fullname");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        modelBuilder.Entity<Taskuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("taskuser_pkey");

            entity.ToTable("taskuser");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Taskid).HasColumnName("taskid");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
