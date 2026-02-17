using System;
using System.Collections.Generic;
using System.Text;

using CliniFlow.Domain.Entities;

namespace CliniFlow.Application.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByDNIAsync(string dni);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
}