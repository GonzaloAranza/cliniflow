using CliniFlow.Application.Interfaces;
using CliniFlow.Domain.Entities;
using CliniFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CliniFlow.Infrastructure.Repositories;

public class MedicalRecordRepository : IMedicalRecordRepository
{
    private readonly ApplicationDbContext _context;

    public MedicalRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(MedicalRecord record)
    {
        await _context.MedicalRecords.AddAsync(record);
    }

    public async Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId)
    {
        return await _context.MedicalRecords
            .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Professional)
                    .ThenInclude(p => p.User)
            .Where(mr => mr.Appointment.PatientId == patientId)
            .OrderByDescending(mr => mr.CreatedAt)
            .ToListAsync();
    }

    public async Task<MedicalRecord?> GetByAppointmentIdAsync(int appointmentId)
    {
        return await _context.MedicalRecords
            .FirstOrDefaultAsync(mr => mr.AppointmentId == appointmentId);
    }
}