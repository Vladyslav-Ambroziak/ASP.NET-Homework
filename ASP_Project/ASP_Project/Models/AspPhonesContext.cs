using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ASP_Project.Models;

public partial class AspPhonesContext : DbContext
{
    public AspPhonesContext()
    {
    }

    public AspPhonesContext(DbContextOptions<AspPhonesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Memory> Memories { get; set; }

    public virtual DbSet<Phone> Phones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=HOME;Database=AspPhones;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC07A3F12724");

            entity.HasIndex(e => e.CompanyName, "UQ__Manufact__9BCE05DC07A0BB0C").IsUnique();

            entity.Property(e => e.CompanyName).HasMaxLength(250);
        });

        modelBuilder.Entity<Memory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Memories__3214EC07883852CD");
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Phones__3214EC07237E7779");

            entity.Property(e => e.Color).HasMaxLength(250);
            entity.Property(e => e.Series).HasMaxLength(250);

            entity.HasOne(d => d.IdManufacturerNavigation).WithMany(p => p.Phones)
                .HasForeignKey(d => d.IdManufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phones__IdManufa__3E52440B");

            entity.HasOne(d => d.IdMemoryNavigation).WithMany(p => p.Phones)
                .HasForeignKey(d => d.IdMemory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phones__IdMemory__3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
