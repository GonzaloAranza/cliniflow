using CliniFlow.Domain.Entities;

namespace CliniFlow.Application.Interfaces;

public interface IMedicalRecordRepository
{
    Task AddAsync(MedicalRecord record);
    Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId);
    Task<MedicalRecord?> GetByAppointmentIdAsync(int appointmentId);
}