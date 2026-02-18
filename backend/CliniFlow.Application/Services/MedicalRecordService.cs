using CliniFlow.Application.DTOs;
using CliniFlow.Application.Interfaces;
using CliniFlow.Domain.Entities;
using CliniFlow.Domain.Enums;

namespace CliniFlow.Application.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _repository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MedicalRecordService(
        IMedicalRecordRepository repository,
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _appointmentRepository = appointmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> CreateAsync(CreateMedicalRecordDto dto)
    {
        // 1. Validar que el turno exista
        var appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId);
        if (appointment == null) throw new KeyNotFoundException("El turno no existe.");

        // 2. Validar que NO tenga ya una historia (Evitar duplicados)
        var existingRecord = await _repository.GetByAppointmentIdAsync(dto.AppointmentId);
        if (existingRecord != null) throw new InvalidOperationException("Este turno ya tiene una historia clínica asociada.");

        // 3. Crear Entidad
        var record = new MedicalRecord
        {
            AppointmentId = dto.AppointmentId,
            Diagnosis = dto.Diagnosis,
            Treatment = dto.Treatment,
            Observations = dto.Observations,
            CreatedAt = DateTime.UtcNow
        };

        // 4. Actualizar estado del turno a "Completado" (Opcional pero recomendado)
        appointment.Status = AppointmentStatus.Completed;

        // 5. Guardar todo en una transacción
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _repository.AddAsync(record);
            await _appointmentRepository.UpdateAsync(appointment); // Guardamos el cambio de estado

            await _unitOfWork.CommitAsync();
            return record.Id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<MedicalRecordDto>> GetHistoryByPatientAsync(int patientId)
    {
        var records = await _repository.GetByPatientIdAsync(patientId);

        return records.Select(r => new MedicalRecordDto
        {
            Id = r.Id,
            Date = r.Appointment!.Date.ToString("yyyy-MM-dd"), // El ! asume que el Include funcionó
            ProfessionalName = r.Appointment.Professional?.User != null
                ? $"{r.Appointment.Professional.User.LastName}, {r.Appointment.Professional.User.FirstName}"
                : "Desconocido",
            Diagnosis = r.Diagnosis,
            Treatment = r.Treatment,
            Observations = r.Observations
        });
    }
}