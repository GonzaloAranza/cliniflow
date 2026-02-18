using CliniFlow.Application.Interfaces;
using CliniFlow.Domain.Entities;
using CliniFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;



namespace CliniFlow.Infrastructure.Repositories;

public class ProfessionalRepository : IProfessionalRepository
{
    private readonly ApplicationDbContext _context;

    public ProfessionalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Professional>> GetAllAsync()
    {
        return await _context.Professionals
            .Include(p => p.User)
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<Professional?> GetByIdAsync(int id)
    {
        return await _context.Professionals
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Professional?> GetByLicenseNumberAsync(string licenseNumber)
    {
        return await _context.Professionals
            .FirstOrDefaultAsync(p => p.LicenseNumber == licenseNumber);
    }

    public async Task AddAsync(Professional professional)
    {
        await _context.Professionals.AddAsync(professional);
        // SaveChanges lo maneja el UnitOfWork, pero si no usas UoW aquí, descomenta:
        await _context.SaveChangesAsync(); 
    }

    public async Task UpdateAsync(Professional professional)
    {
        _context.Professionals.Update(professional);
         await _context.SaveChangesAsync(); // Dejamos que el UnitOfWork confirme
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var professional = await GetByIdAsync(id);
        if (professional != null)
        {
            professional.IsActive = false; // Soft Delete
            _context.Professionals.Update(professional);
             await _context.SaveChangesAsync(); // Dejamos que el UnitOfWork confirme
        }
    }
}