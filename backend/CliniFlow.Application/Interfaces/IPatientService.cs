using System;
using System.Collections.Generic;
using System.Text;
using CliniFlow.Application.DTOs;



namespace CliniFlow.Application.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientListDto>> GetAllAsync();
    Task<PatientDetailDto?> GetByDNIAsync(string dni);
    Task<PatientDetailDto> CreateAsync(CreatePatientDto dto);
    Task<PatientDetailDto?> UpdateAsync(string dni, UpdatePatientDto dto);
    Task<bool> DeleteAsync(string dni);
}