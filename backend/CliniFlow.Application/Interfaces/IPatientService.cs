using System;
using System.Collections.Generic;
using System.Text;

using CliniFlow.Application.DTOs;

namespace CliniFlow.Application.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllAsync();
    Task<PatientDto?> GetByIdAsync(int id);
    Task<PatientDto> CreateAsync(CreatePatientDto dto);
    Task<PatientDto?> UpdateAsync(int id, UpdatePatientDto dto);
    Task<bool> DeleteAsync(int id);
}