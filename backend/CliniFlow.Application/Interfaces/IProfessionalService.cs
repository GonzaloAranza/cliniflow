using CliniFlow.Application.DTOs;

namespace CliniFlow.Application.Interfaces;

public interface IProfessionalService
{
    Task<IEnumerable<ProfessionalDto>> GetAllAsync();
    Task<ProfessionalDetailDto?> GetByIdAsync(int id);
    Task<int> RegisterProfessionalAsync(CreateProfessionalDto dto);
    Task UpdateAsync(int id, UpdateProfessionalDto dto);
    Task DeleteAsync(int id);
}