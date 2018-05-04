using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StockTracking.Models
{
    public partial class InvesmentContext : DbContext
    {
        public virtual DbSet<Estimate> Estimate { get; set; }
        public virtual DbSet<InvesmentCompany> InvesmentCompany { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=PC670\SQLEXPRESS;Database=Invesment;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estimate>(entity =>
            {
                entity.Property(e => e.ClosingPrice).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.OpeningPrice).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.Period)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.TargetPrice).HasColumnType("decimal(18, 6)");

                entity.HasOne(d => d.InvesmentCompany)
                    .WithMany(p => p.Estimate)
                    .HasForeignKey(d => d.InvesmentCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estimate_InvesmentCompany");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.Estimate)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estimate_Stock");
            });

            modelBuilder.Entity<InvesmentCompany>(entity =>
            {
                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Group)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });
        }
    }
}
