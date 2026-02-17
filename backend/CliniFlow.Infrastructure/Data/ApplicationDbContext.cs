using CliniFlow.Domain.Entities;
using CliniFlow.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CliniFlow.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Professional> Professionals => Set<Professional>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ===== USERS =====
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.DNI).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.DNI).IsUnique();

            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);

            entity.Property(e => e.Role).IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);

            entity.HasOne(e => e.Professional)
                .WithOne(p => p.User)
                .HasForeignKey<Professional>(p => p.UserId);
        });

        // ===== PROFESSIONALS =====
        modelBuilder.Entity<Professional>(entity =>
        {
            entity.ToTable("Professionals");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId).IsRequired();
            entity.HasIndex(e => e.UserId).IsUnique();

            entity.Property(e => e.Specialty).HasMaxLength(100);

            entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.LicenseNumber).IsUnique();

            entity.Property(e => e.Phone).IsRequired().HasMaxLength(30);
        });

        // ===== PATIENTS =====
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patients");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.DNI).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.DNI).IsUnique();

            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DateOfBirth).IsRequired();

            entity.Property(e => e.Gender).IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.Phone).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(300);
            entity.Property(e => e.HealthInsurance).IsRequired().HasMaxLength(100);
        });

        // ===== APPOINTMENTS =====
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("Appointments");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.AppointmentCode).IsRequired().HasMaxLength(30);
            entity.HasIndex(e => e.AppointmentCode).IsUnique();

            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.StartTime).IsRequired();

            entity.Property(e => e.Status).IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(AppointmentStatus.Scheduled);

            entity.Property(e => e.CancellationReason).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(500);

            // Índice compuesto: agenda de un profesional en una fecha
            entity.HasIndex(e => new { e.Date, e.ProfessionalId });
            entity.HasIndex(e => e.PatientId);

            // Relaciones
            entity.HasOne(e => e.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Professional)
                .WithMany(p => p.Appointments)
                .HasForeignKey(e => e.ProfessionalId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedByUser)
                .WithMany(u => u.CreatedAppointments)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===== MEDICAL RECORDS =====
        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.ToTable("MedicalRecords");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.RecordCode).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.RecordCode).IsUnique();

            entity.Property(e => e.Diagnosis).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Treatment).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Observations).HasMaxLength(2000);

            entity.HasIndex(e => e.AppointmentId).IsUnique();
            entity.HasIndex(e => e.PatientId);

            // Relaciones
            entity.HasOne(e => e.Appointment)
                .WithOne(a => a.MedicalRecord)
                .HasForeignKey<MedicalRecord>(e => e.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Professional)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(e => e.ProfessionalId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}