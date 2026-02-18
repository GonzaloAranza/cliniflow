using CliniFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CliniFlow.Application.DTOs;



public class ProfessionalDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty; 
    public string Specialty { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
}

public class ProfessionalListDto
{
    public string DNI { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public string? Specialty { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string HealthInsurance { get; set; } = string.Empty;
}
public class CreateProfessionalDto
{
    // Datos de Usuario (Login)
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public string DNI { get; set; } = string.Empty; // <--- ¡AGREGADO!
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;

    // Datos del Profesional (Negocio)
    [Required] public string LicenseNumber { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    [Required] public string Phone { get; set; } = string.Empty;
}

public class ProfessionalDetailDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    // Datos personales (Vienen de la entidad User)
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    // Agregamos DNI si tu entidad User lo tiene (asumo que sí por tu comentario)
     public string DNI { get; set; } = string.Empty; 

    // Datos profesionales (Vienen de la entidad Professional)
    public string LicenseNumber { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}



public class UpdateProfessionalDto
{
    // Permitimos editar info básica del usuario
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;

    // Y datos del profesional
    [Required] public string Phone { get; set; } = string.Empty;
    public string? Specialty { get; set; }
}