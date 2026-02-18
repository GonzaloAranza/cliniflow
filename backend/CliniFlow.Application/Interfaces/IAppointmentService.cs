using CliniFlow.Application.DTOs;

namespace CliniFlow.Application.Interfaces;

public interface IAppointmentService
{
    Task<int> CreateAppointmentAsync(CreateAppointmentDto dto);
    Task<AppointmentDetailDto?> GetByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetByProfessionalAsync(int professionalId, DateOnly? date = null);
    Task CancelAppointmentAsync(int id, string reason);
    // Task CompleteAppointmentAsync(int id); // Para más adelante
}