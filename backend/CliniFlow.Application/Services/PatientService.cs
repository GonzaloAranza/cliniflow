using System;
using System.Collections.Generic;
using System.Text;

using CliniFlow.Application.DTOs;
using CliniFlow.Application.Interfaces;
using CliniFlow.Domain.Entities;

namespace CliniFlow.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync()
    {
        var patients = await _patientRepository.GetAllAsync();
        return patients.Select(MapToDto);
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);
        return patient == null ? null : MapToDto(patient);
    }

    public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
    {
        // Validar que no exista otro paciente con el mismo DNI
        var existing = await _patientRepository.GetByDNIAsync(dto.DNI);
        if (existing != null)
            throw new InvalidOperationException($"Ya existe un paciente con DNI {dto.DNI}");

        var patient = new Patient
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            DNI = dto.DNI.Trim(),
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            HealthInsurance = dto.HealthInsurance,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var created = await _patientRepository.CreateAsync(patient);
        return MapToDto(created);
    }

    public async Task<PatientDto?> UpdateAsync(int id, UpdatePatientDto dto)
    {
        var patient = await _patientRepository.GetByIdAsync(id);
        if (patient == null) return null;

        // Validar que el DNI no pertenezca a otro paciente
        var existingWithDNI = await _patientRepository.GetByDNIAsync(dto.DNI);
        if (existingWithDNI != null && existingWithDNI.Id != id)
            throw new InvalidOperationException($"Ya existe otro paciente con DNI {dto.DNI}");

        patient.FirstName = dto.FirstName.Trim();
        patient.LastName = dto.LastName.Trim();
        patient.DNI = dto.DNI.Trim();
        patient.DateOfBirth = dto.DateOfBirth;
        patient.Gender = dto.Gender;
        patient.Phone = dto.Phone;
        patient.Email = dto.Email;
        patient.Address = dto.Address;
        patient.HealthInsurance = dto.HealthInsurance;

        await _patientRepository.UpdateAsync(patient);
        return MapToDto(patient);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);
        if (patient == null) return false;

        await _patientRepository.DeleteAsync(patient);
        return true;
    }

    // Mapeo manual de Entity a DTO
    private static PatientDto MapToDto(Patient patient)
    {
        return new PatientDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DNI = patient.DNI,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            Phone = patient.Phone,
            Email = patient.Email,
            Address = patient.Address,
            HealthInsurance = patient.HealthInsurance,
            CreatedAt = patient.CreatedAt,
            IsActive = patient.IsActive
        };
    }
}