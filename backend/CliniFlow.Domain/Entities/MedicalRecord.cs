using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliniFlow.Domain.Entities;

public class MedicalRecord
{
    [Key]
    public int Id { get; set; }

    // ÚNICA RELACIÓN NECESARIA: El Turno
    [Required]
    public int AppointmentId { get; set; }

    [ForeignKey("AppointmentId")]
    public Appointment? Appointment { get; set; }

    // Datos Médicos
    [Required]
    public string Diagnosis { get; set; } = string.Empty;

    [Required]
    public string Treatment { get; set; } = string.Empty;

    public string? Observations { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // UpdatedAt es opcional, dejémoslo si quieres permitir ediciones futuras
    public DateTime? UpdatedAt { get; set; }
}