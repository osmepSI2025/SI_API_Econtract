using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SME_API_Econtract.Entities;

public partial class Si_EcontractDBContext : DbContext
{
    public Si_EcontractDBContext()
    {
    }

    public Si_EcontractDBContext(DbContextOptions<Si_EcontractDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MApiInformation> MApiInformations { get; set; }

    public virtual DbSet<MProjectContract> MProjectContracts { get; set; }

    public virtual DbSet<MScheduledJob> MScheduledJobs { get; set; }

    public virtual DbSet<TInstallmentDetail> TInstallmentDetails { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("SME_Econtract");

        modelBuilder.Entity<MApiInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MApiInformation");

            entity.ToTable("M_ApiInformation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApiKey).HasMaxLength(150);
            entity.Property(e => e.AuthorizationType).HasMaxLength(50);
            entity.Property(e => e.Bearer).HasColumnType("ntext");
            entity.Property(e => e.ContentType).HasMaxLength(150);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MethodType).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(150);
            entity.Property(e => e.ServiceNameCode).HasMaxLength(250);
            entity.Property(e => e.ServiceNameTh).HasMaxLength(250);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Urldevelopment).HasColumnName("URLDevelopment");
            entity.Property(e => e.Urlproduction).HasColumnName("URLProduction");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<MProjectContract>(entity =>
        {
            entity.HasKey(e => e.ProjectCode).HasName("PK__M_Projec__C3B33FE4B9843089");

            entity.ToTable("M_ProjectContract");

            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .HasColumnName("projectCode");
            entity.Property(e => e.AllocatedBudget)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("allocatedBudget");
            entity.Property(e => e.ContractingPartyName)
                .HasMaxLength(255)
                .HasColumnName("contractingPartyName");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("endDate");
            entity.Property(e => e.Installments).HasColumnName("installments");
            entity.Property(e => e.ProjectBudget)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("projectBudget");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(255)
                .HasColumnName("projectName");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("startDate");
        });

        modelBuilder.Entity<MScheduledJob>(entity =>
        {
            entity.ToTable("M_ScheduledJobs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.JobName).HasMaxLength(150);
        });

        modelBuilder.Entity<TInstallmentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__T_Instal__3213E83F41E3ED32");

            entity.ToTable("T_InstallmentDetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.InstallmentsDate)
                .HasColumnType("date")
                .HasColumnName("installmentsDate");
            entity.Property(e => e.InstallmentsNo).HasColumnName("installmentsNo");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .HasColumnName("projectCode");

            entity.HasOne(d => d.ProjectCodeNavigation).WithMany(p => p.TInstallmentDetails)
                .HasForeignKey(d => d.ProjectCode)
                .HasConstraintName("FK__T_Install__proje__1B0907CE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
