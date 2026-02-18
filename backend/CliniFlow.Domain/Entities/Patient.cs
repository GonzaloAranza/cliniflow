using System;
using System.Collections.Generic;
using System.Text;

using CliniFlow.Domain.Enums;

namespace CliniFlow.Domain.Entities;

public class Patient
{
    public int Id { get; set; }
    public string DNI { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string HealthInsurance { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navegación
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}