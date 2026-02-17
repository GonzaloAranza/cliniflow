using System;
using System.Collections.Generic;
using System.Text;

using CliniFlow.Domain.Enums;

namespace CliniFlow.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public string AppointmentCode { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public int ProfessionalId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public string? CancellationReason { get; set; }
    public string? Notes { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navegación
    public Patient Patient { get; set; } = null!;
    public Professional Professional { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
    public MedicalRecord? MedicalRecord { get; set; }
}