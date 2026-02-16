using System;
using System.Collections.Generic;
using System.Text;

namespace CliniFlow.Application.DTOs;

// Lo que DEVOLVEMOS al frontend
public class PatientDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string DNI { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Age => CalculateAge();
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? HealthInsurance { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    private int CalculateAge()
    {
        var today = DateTime.UtcNow;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}

// Lo que RECIBIMOS del frontend para CREAR
public class CreatePatientDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? HealthInsurance { get; set; }
}

// Lo que RECIBIMOS del frontend para ACTUALIZAR
public class UpdatePatientDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? HealthInsurance { get; set; }
}