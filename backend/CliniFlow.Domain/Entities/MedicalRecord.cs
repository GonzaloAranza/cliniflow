using System;
using System.Collections.Generic;
using System.Text;

namespace CliniFlow.Domain.Entities;

public class MedicalRecord
{
    public int Id { get; set; }
    public string RecordCode { get; set; } = string.Empty;
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int ProfessionalId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string? Observations { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navegación
    public Appointment Appointment { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public Professional Professional { get; set; } = null!;
}