using System;
using System.Collections.Generic;
using System.Text;

using CliniFlow.Domain.Entities;


namespace CliniFlow.Application.Interfaces;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllAsync();
    Task<Patient?> GetByDNIAsync(string dni);
    Task<Patient> CreateAsync(Patient patient);
    Task UpdateAsync(Patient patient);
    Task<Patient?> GetByIdAsync(int id);
}