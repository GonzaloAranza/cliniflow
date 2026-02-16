using System;
using System.Collections.Generic;
using System.Text;
using CliniFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CliniFlow.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patients");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.DNI)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(e => e.DNI)
                .IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(200);

            entity.Property(e => e.Phone)
                .HasMaxLength(30);

            entity.Property(e => e.Gender)
                .HasMaxLength(30);

            entity.Property(e => e.Address)
                .HasMaxLength(300);

            entity.Property(e => e.HealthInsurance)
                .HasMaxLength(100);
        });
    }
}