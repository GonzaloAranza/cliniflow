using CliniFlow.Application.DTOs;

namespace CliniFlow.Application.Interfaces;

public interface IMedicalRecordService
{
    Task<int> CreateAsync(CreateMedicalRecordDto dto);
    Task<IEnumerable<MedicalRecordDto>> GetHistoryByPatientAsync(int patientId);
}