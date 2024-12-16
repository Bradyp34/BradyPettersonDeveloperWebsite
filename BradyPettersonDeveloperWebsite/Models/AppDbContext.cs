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

    public virtual DbSet<Featuretask> Featuretasks { get; set; }

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
        });

        modelBuilder.Entity<Featuretask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("featuretask_pkey");

            entity.HasOne(d => d.Feature).WithMany(p => p.Featuretasks).HasConstraintName("featuretask_featureid_fkey");

            entity.HasOne(d => d.Task).WithMany(p => p.Featuretasks).HasConstraintName("featuretask_taskid_fkey");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("project_pkey");
        });

        modelBuilder.Entity<Projecttask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('task_id_seq'::regclass)");

            entity.HasOne(d => d.Assignee).WithMany(p => p.Projecttasks).HasConstraintName("task_assigneeid_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.Projecttasks).HasConstraintName("task_projectid_fkey");
        });

        modelBuilder.Entity<Projectuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projectuser_pkey");

            entity.HasOne(d => d.Project).WithMany(p => p.Projectusers).HasConstraintName("projectuser_projectid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Projectusers).HasConstraintName("projectuser_userid_fkey");
        });

        modelBuilder.Entity<Siteuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"User_id_seq\"'::regclass)");
        });

        modelBuilder.Entity<Taskuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("taskuser_pkey");

            entity.HasOne(d => d.Task).WithMany(p => p.Taskusers).HasConstraintName("taskuser_taskid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Taskusers).HasConstraintName("taskuser_userid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
