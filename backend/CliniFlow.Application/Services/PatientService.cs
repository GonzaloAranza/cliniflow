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

    public async Task<IEnumerable<PatientListDto>> GetAllAsync()
    {
        var patients = await _patientRepository.GetAllAsync();
        return patients.Select(MapToListDto);
    }

    public async Task<PatientDetailDto?> GetByDNIAsync(string dni)
    {
        var patient = await _patientRepository.GetByDNIAsync(dni);
        return patient == null ? null : MapToDetailDto(patient);
    }

    public async Task<PatientDetailDto> CreateAsync(CreatePatientDto dto)
    {
        var existing = await _patientRepository.GetByDNIAsync(dto.DNI);
        if (existing != null)
            throw new InvalidOperationException($"Ya existe un paciente con DNI {dto.DNI}");

        var patient = new Patient
        {
            DNI = dto.DNI.Trim(),
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Phone = dto.Phone.Trim(),
            Email = dto.Email.Trim(),
            Address = dto.Address.Trim(),
            HealthInsurance = dto.HealthInsurance.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var created = await _patientRepository.CreateAsync(patient);
        return MapToDetailDto(created);
    }

    public async Task<PatientDetailDto?> UpdateAsync(string dni, UpdatePatientDto dto)
    {
        var patient = await _patientRepository.GetByDNIAsync(dni);
        if (patient == null) return null;

        patient.FirstName = dto.FirstName.Trim();
        patient.LastName = dto.LastName.Trim();
        patient.DateOfBirth = dto.DateOfBirth;
        patient.Gender = dto.Gender;
        patient.Phone = dto.Phone.Trim();
        patient.Email = dto.Email.Trim();
        patient.Address = dto.Address.Trim();
        patient.HealthInsurance = dto.HealthInsurance.Trim();
        patient.UpdatedAt = DateTime.UtcNow;

        await _patientRepository.UpdateAsync(patient);
        return MapToDetailDto(patient);
    }

    public async Task<bool> DeleteAsync(string dni)
    {
        var patient = await _patientRepository.GetByDNIAsync(dni);
        if (patient == null) return false;

        patient.IsActive = false;
        patient.UpdatedAt = DateTime.UtcNow;
        await _patientRepository.UpdateAsync(patient);
        return true;
    }

    private static int CalculateAge(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age)) age--;
        return age;
    }

    private static PatientListDto MapToListDto(Patient patient)
    {
        return new PatientListDto
        {
            DNI = patient.DNI,
            FullName = $"{patient.LastName}, {patient.FirstName}",
            Age = CalculateAge(patient.DateOfBirth),
            Gender = patient.Gender,
            Phone = patient.Phone,
            HealthInsurance = patient.HealthInsurance
        };
    }

    private static PatientDetailDto MapToDetailDto(Patient patient)
    {
        return new PatientDetailDto
        {
            DNI = patient.DNI,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            FullName = $"{patient.LastName}, {patient.FirstName}",
            DateOfBirth = patient.DateOfBirth,
            Age = CalculateAge(patient.DateOfBirth),
            Gender = patient.Gender,
            Phone = patient.Phone,
            Email = patient.Email,
            Address = patient.Address,
            HealthInsurance = patient.HealthInsurance,
            CreatedAt = patient.CreatedAt
        };
    }
}