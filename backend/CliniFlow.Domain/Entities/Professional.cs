using System;
using System.Collections.Generic;
using System.Text;

using CliniFlow.Domain.Enums;

namespace CliniFlow.Domain.Entities;

public class Professional
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Specialty { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navegación
    public User User { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}