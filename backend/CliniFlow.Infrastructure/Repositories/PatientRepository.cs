using System;
using System.Collections.Generic;
using System.Text;
using CliniFlow.Application.Interfaces;
using CliniFlow.Domain.Entities;
using CliniFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace CliniFlow.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ApplicationDbContext _context;

    public PatientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .Where(p => p.IsActive)
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();
    }

    public async Task<Patient?> GetByDNIAsync(string dni)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.DNI == dni && p.IsActive);
    }

    public async Task<Patient> CreateAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
    }
}