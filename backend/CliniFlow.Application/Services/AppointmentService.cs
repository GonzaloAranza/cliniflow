using CliniFlow.Application.DTOs;
using CliniFlow.Application.Interfaces;
using CliniFlow.Domain.Entities;
using CliniFlow.Domain.Enums;
using CliniFlow.Application.Utils;
namespace CliniFlow.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IProfessionalRepository professionalRepository,
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _professionalRepository = professionalRepository;
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> CreateAppointmentAsync(CreateAppointmentDto dto)
    {
        // 1. Validaciones previas (Hora, Fecha, etc.)
        if (!TimeOnly.TryParse(dto.StartTime, out TimeOnly parsedStartTime))
        {
            throw new InvalidOperationException("Formato de hora inválido. Use HH:mm.");
        }

        if (dto.Date < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new InvalidOperationException("No se pueden agendar turnos en el pasado.");
        }

        var isTaken = await _appointmentRepository.IsSlotTakenAsync(dto.ProfessionalId, dto.Date, parsedStartTime);
        if (isTaken)
        {
            throw new InvalidOperationException("El profesional ya tiene un turno en ese horario.");
        }

        // Buscamos las entidades relacionadas
        var professional = await _professionalRepository.GetByIdAsync(dto.ProfessionalId);
        if (professional == null) throw new KeyNotFoundException("Profesional no encontrado.");

        var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
        if (patient == null) throw new KeyNotFoundException("Paciente no encontrado.");


        // ---------------------------------------------------------
        // 2. GENERACIÓN DE CÓDIGO ÚNICO (Lógica de Aerolínea)
        // ---------------------------------------------------------
        string appointmentCode;
        bool isUnique = false;

        // Bucle de seguridad: Sigue intentando hasta encontrar uno libre
        // (En la vida real, esto corre 1 vez el 99.99% de los casos)
        do
        {
            appointmentCode = CodeGenerator.GenerateAppointmentCode(6); // Ej: "X9J2M4"

            // Verificamos en BD que nadie más lo tenga
            var exists = await _appointmentRepository.ExistsByCodeAsync(appointmentCode);

            if (!exists) isUnique = true;

        } while (!isUnique);


        // 3. Crear Entidad
        var appointment = new Appointment
        {
            PatientId = dto.PatientId,
            ProfessionalId = dto.ProfessionalId,
            Date = dto.Date,
            StartTime = parsedStartTime,
            Status = AppointmentStatus.Scheduled,
            Notes = dto.Reason,

            // Asignamos el código generado
            AppointmentCode = appointmentCode,

            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = professional.UserId // El ID real del usuario médico
        };

        // 4. Guardar
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _appointmentRepository.AddAsync(appointment);
            await _unitOfWork.CommitAsync();
            return appointment.Id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<AppointmentDetailDto?> GetByIdAsync(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null) return null;

        return MapToDetailDto(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetByProfessionalAsync(int professionalId, DateOnly? date = null)
    {
        var appointments = await _appointmentRepository.GetByProfessionalIdAsync(professionalId, date);
        return appointments.Select(MapToDto);
    }

    public async Task CancelAppointmentAsync(int id, string reason)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null) throw new KeyNotFoundException("Turno no encontrado.");

            // Validar si ya estaba cancelado o completado
            if (appointment.Status != AppointmentStatus.Scheduled)
            {
                throw new InvalidOperationException($"No se puede cancelar un turno con estado {appointment.Status}");
            }

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancellationReason = reason;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAsync(appointment);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    // --- MÉTODOS PRIVADOS DE MAPEO (Para no repetir código) ---

    private static AppointmentDto MapToDto(Appointment a)
    {
        return new AppointmentDto
        {
            Id = a.Id,
            Date = a.Date.ToString("yyyy-MM-dd"),
            Time = a.StartTime.ToString("HH:mm"),
            PatientName = $"{a.Patient.LastName}, {a.Patient.FirstName}",
            // Validamos nulos por seguridad, aunque el Include debería traerlos
            ProfessionalName = a.Professional?.User != null
                ? $"{a.Professional.User.LastName}, {a.Professional.User.FirstName}"
                : "Desconocido",
            Status = a.Status
        };
    }

    private static AppointmentDetailDto MapToDetailDto(Appointment a)
    {
        var dto = new AppointmentDetailDto
        {
            Id = a.Id,
            Date = a.Date.ToString("yyyy-MM-dd"),
            Time = a.StartTime.ToString("HH:mm"),
            PatientName = $"{a.Patient.LastName}, {a.Patient.FirstName}",
            ProfessionalName = a.Professional?.User != null
                ? $"{a.Professional.User.LastName}, {a.Professional.User.FirstName}"
                : "Desconocido",
            Status = a.Status,
            PatientId = a.PatientId,
            ProfessionalId = a.ProfessionalId,
            Notes = a.Notes,
            CancellationReason = a.CancellationReason
        };
        return dto;
    }
}