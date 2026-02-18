using System;
using System.Collections.Generic;
using System.Text;

using CliniFlow.Domain.Entities;

namespace CliniFlow.Application.Interfaces;

public interface IAppointmentRepository
{
    // --- LECTURA ---
    Task<Appointment?> GetByIdAsync(int id);

    // Para la agenda del médico (opcionalmente filtramos por fecha específica)
    Task<IEnumerable<Appointment>> GetByProfessionalIdAsync(int professionalId, DateOnly? date = null);

    // Para el historial del paciente
    Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);

    // --- VALIDACIÓN DE NEGOCIO (EL SUPERPODER) ---
    // Devuelve true si el médico ya tiene un turno activo en esa fecha y hora
    Task<bool> IsSlotTakenAsync(int professionalId, DateOnly date, TimeOnly startTime);
    Task<bool> ExistsByCodeAsync(string code);

    // --- ESCRITURA ---
    Task AddAsync(Appointment appointment);
    Task UpdateAsync(Appointment appointment);
}