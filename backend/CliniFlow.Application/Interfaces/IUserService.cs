using CliniFlow.Application.DTOs;
using CliniFlow.Domain.Entities;
using CliniFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliniFlow.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserListDto>> GetAllAsync();
    Task<UserDetailDto?> GetByDNIAsync(string dni);
    Task<UserDetailDto> CreateAsync(CreateUserDto dto);
    Task<UserDetailDto?> UpdateAsync(string dni, UpdateUserDto dto);
    Task<bool> DeleteAsync(string dni);

}