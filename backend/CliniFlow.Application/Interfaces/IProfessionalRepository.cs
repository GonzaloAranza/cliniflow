using CliniFlow.Domain.Entities;

namespace CliniFlow.Application.Interfaces;

public interface IProfessionalRepository
{
    // Métodos de Lectura
    Task<IEnumerable<Professional>> GetAllAsync();
    Task<Professional?> GetByIdAsync(int id);
    Task<Professional?> GetByLicenseNumberAsync(string licenseNumber);

    // Métodos de Escritura (Estos son los que te faltaban)
    Task AddAsync(Professional professional);
    Task UpdateAsync(Professional professional);
    Task DeleteAsync(int id);
}