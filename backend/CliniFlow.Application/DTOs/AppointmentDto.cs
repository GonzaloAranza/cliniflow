using CliniFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CliniFlow.Application.DTOs;

public class CreateAppointmentDto
{
    [Required]
    public int PatientId { get; set; }

    [Required]
    public int ProfessionalId { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required(ErrorMessage = "La hora es obligatoria")]
    [RegularExpression(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "El formato debe ser HH:mm (ej: 09:30)")]
    public string StartTime { get; set; } = string.Empty;

    public string? Reason { get; set; } // Motivo de la consulta
}

public class AppointmentDto
{
    public int Id { get; set; }
    public string Date { get; set; } = string.Empty; // Devolvemos como string para evitar problemas de formato en JS
    public string Time { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string ProfessionalName { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }
}

public class AppointmentDetailDto : AppointmentDto
{
    public int PatientId { get; set; }
    public int ProfessionalId { get; set; }
    public string? Notes { get; set; }
    public string? CancellationReason { get; set; }
    // Aquí podrías agregar más info, como datos del consultorio, etc.
}