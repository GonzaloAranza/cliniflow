using System;
using System.Collections.Generic;
using System.Text;
using CliniFlow.Domain.Enums;

namespace CliniFlow.Application.DTOs;

// Para la LISTA de pacientes (GET /api/patients) — datos resumidos
public class PatientListDto
{
    public string DNI { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public Gender Gender { get; set; }

    public string Phone { get; set; } = string.Empty;
    public string HealthInsurance { get; set; } = string.Empty;
}

// Para el DETALLE de un paciente (GET /api/patients/{dni}) — datos completos
public class PatientDetailDto
{
    public string DNI { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string HealthInsurance { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

// Para CREAR un paciente (POST)
public class CreatePatientDto
{
    public string DNI { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string HealthInsurance { get; set; } = string.Empty;
}

// Para ACTUALIZAR un paciente (PUT)
public class UpdatePatientDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string HealthInsurance { get; set; } = string.Empty;
}