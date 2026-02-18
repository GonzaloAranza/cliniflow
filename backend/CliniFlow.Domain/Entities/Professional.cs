using CliniFlow.Domain.Enums;

namespace CliniFlow.Domain.Entities;

public class Professional
{
    public int Id { get; set; }
    public int UserId { get; set; } // FK al Usuario
    public string LicenseNumber { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }


    // Relaciones
    public User User { get; set; } = null!;
    // Descomenta la colección de turnos si ya tienes la entidad Appointment
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}