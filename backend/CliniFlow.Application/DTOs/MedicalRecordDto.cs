using System.ComponentModel.DataAnnotations;

namespace CliniFlow.Application.DTOs;

public class CreateMedicalRecordDto
{
    [Required]
    public int AppointmentId { get; set; }

    [Required(ErrorMessage = "El diagnóstico es obligatorio")]
    public string Diagnosis { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tratamiento es obligatorio")]
    public string Treatment { get; set; } = string.Empty;

    public string? Observations { get; set; }
}

public class MedicalRecordDto
{
    public int Id { get; set; }
    public string Date { get; set; } = string.Empty; // Fecha del turno
    public string ProfessionalName { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string? Observations { get; set; }
}