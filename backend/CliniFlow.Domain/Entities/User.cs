using System;
using System.Collections.Generic;
using System.Text;

using CliniFlow.Domain.Enums;

namespace CliniFlow.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string DNI { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navegación
    public Professional? Professional { get; set; }
    public ICollection<Appointment> CreatedAppointments { get; set; } = new List<Appointment>();
}