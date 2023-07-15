using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExcelAutomation.Data;

public partial class ExcelAutomationContext : DbContext
{
    public ExcelAutomationContext()
    {
    }

    public ExcelAutomationContext(DbContextOptions<ExcelAutomationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectDetail> ProjectDetails { get; set; }

    public virtual DbSet<PlanElevationReferance> PlanElevationReferances { get; set; }
    public virtual DbSet<ProjectGroup> ProjectGroups { get; set; }
    public virtual DbSet<PlanElevationText> PlanElevationText { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ExcelAutomation");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project");

            entity.Property(e => e.ProjectName).HasMaxLength(200);

            entity.Property(e => e.NominalCf).IsRequired(false).HasMaxLength(200);

            entity.Property(e => e.ActualCf).IsRequired(false).HasMaxLength(200);

            entity.Property(e => e.CreatedDate).IsRequired(false);

            entity.Property(e => e.LineItemTotal).IsRequired(false);

            entity.Property(e => e.RevisionDate).IsRequired(false);

            entity.Property(e => e.ContactSpecs).IsRequired(false);
        });

        modelBuilder.Entity<ProjectDetail>(entity =>
        {
            entity.ToTable("ProjectDetail");

            entity.Property(e => e.ActCfpcs)
                .HasMaxLength(50)
                .HasColumnName("ActCFPcs");
            entity.Property(e => e.ActSfcflf)
                .HasMaxLength(50)
                .HasColumnName("ActSFCFLF");
            entity.Property(e => e.DetailPage).HasMaxLength(50);
            entity.Property(e => e.DispositionSpecialNote).HasMaxLength(1000);
            entity.Property(e => e.Height).HasMaxLength(50);
            entity.Property(e => e.ImagePath).HasMaxLength(50);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.Length).HasMaxLength(50);
            entity.Property(e => e.MoldQty).HasMaxLength(50);
            entity.Property(e => e.NomCflf)
                .HasMaxLength(50)
                .HasColumnName("NomCFLF");
            entity.Property(e => e.NomCfpcs)
                .HasMaxLength(50)
                .HasColumnName("NomCFPcs");
            entity.Property(e => e.Pieces).HasMaxLength(50);
            entity.Property(e => e.TakeOffColor).HasMaxLength(50);
            entity.Property(e => e.TotalActCf)
                .HasMaxLength(50)
                .HasColumnName("TotalActCF");
            entity.Property(e => e.TotalLf)
                .HasMaxLength(50)
                .HasColumnName("TotalLF");
            entity.Property(e => e.TotalNomCf)
                .HasMaxLength(50)
                .HasColumnName("TotalNomCF");
            entity.Property(e => e.Wd)
                .HasMaxLength(50)
                .HasColumnName("WD");
            entity.Property(e => e.Width).HasMaxLength(50).HasMaxLength(50);

            entity.Property(e => e.LineItemCharge).IsRequired(false);

            entity.Property(e => e.TotalActualNominalValue).IsRequired(false).HasMaxLength(50);
            entity.Property(e => e.Category).IsRequired(false).HasMaxLength(50);

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectDetails)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectDetail_Project");
        });

        modelBuilder.Entity<PlanElevationReferance>(entity =>
        {
            entity.ToTable("PlanElevationReferance");

            entity.Property(e => e.PlanElevationValue).HasMaxLength(500);

            entity.Property(e => e.LFValue).HasMaxLength(50);

            entity.Property(e => e.ImagePath).IsRequired(false).HasMaxLength(1000); 

            entity.Property(e => e.PageRefPath).IsRequired(false).HasMaxLength(1000);

            entity.HasOne(d => d.ProjectDetail).WithMany(p => p.PlanElevationReferances)
                .HasForeignKey(d => d.ProjectDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlanElevationReferance_ProjectDetail");
        });

        modelBuilder.Entity<ProjectGroup>(entity => {
            entity.ToTable("ProjectGroup");
           
        });

        modelBuilder.Entity<PlanElevationText>(entity => {
            entity.ToTable("PlanElevationText");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
