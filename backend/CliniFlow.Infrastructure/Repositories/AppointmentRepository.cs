using System;
using System.Collections.Generic;
using System.Text;
using CliniFlow.Application.Interfaces;
using CliniFlow.Domain.Entities;
using CliniFlow.Domain.Enums; // Para ver el Status
using CliniFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CliniFlow.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _context;

    public AppointmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _context.Appointments
            .Include(a => a.Patient) // Traemos datos del paciente
            .Include(a => a.Professional)
                .ThenInclude(p => p.User) // Traemos datos del usuario médico (Nombre, Apellido)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Appointment>> GetByProfessionalIdAsync(int professionalId, DateOnly? date = null)
    {
        var query = _context.Appointments
            .Include(a => a.Patient)
            .Where(a => a.ProfessionalId == professionalId);

        // Si nos pasan una fecha, filtramos por ella (útil para "Ver agenda de hoy")
        if (date.HasValue)
        {
            query = query.Where(a => a.Date == date.Value);
        }

        return await query
            .OrderBy(a => a.Date)
            .ThenBy(a => a.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId)
    {
        return await _context.Appointments
            .Include(a => a.Professional)
                .ThenInclude(p => p.User)
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.Date) // Lo más reciente primero
            .ToListAsync();
    }

    // EL MÉTODO CRÍTICO: Evita solapamientos
    public async Task<bool> IsSlotTakenAsync(int professionalId, DateOnly date, TimeOnly startTime)
    {
        return await _context.Appointments
            .AnyAsync(a =>
                a.ProfessionalId == professionalId &&
                a.Date == date &&
                a.StartTime == startTime &&
                a.Status != AppointmentStatus.Cancelled // Si está cancelado, el horario cuenta como libre
            );
    }

    public async Task AddAsync(Appointment appointment)
    {
        await _context.Appointments.AddAsync(appointment);
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Appointments.AnyAsync(a => a.AppointmentCode == code);
    }
}