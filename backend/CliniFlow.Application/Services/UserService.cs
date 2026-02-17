using System;
using System.Collections.Generic;
using System.Text;

using CliniFlow.Application.DTOs;
using CliniFlow.Application.Interfaces;
using CliniFlow.Domain.Entities;

namespace CliniFlow.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserListDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToListDto);
    }

    public async Task<UserDetailDto?> GetByDNIAsync(string dni)
    {
        var user = await _userRepository.GetByDNIAsync(dni);
        return user == null ? null : MapToDetailDto(user);
    }

    public async Task<UserDetailDto> CreateAsync(CreateUserDto dto)
    {
        var existingDNI = await _userRepository.GetByDNIAsync(dto.DNI);
        if (existingDNI != null)
            throw new InvalidOperationException($"Ya existe un usuario con DNI {dto.DNI}");

        var existingEmail = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingEmail != null)
            throw new InvalidOperationException($"Ya existe un usuario con email {dto.Email}");

        var user = new User
        {
            DNI = dto.DNI.Trim(),
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Email = dto.Email.Trim().ToLower(),
            PasswordHash = HashPassword(dto.Password),
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var created = await _userRepository.CreateAsync(user);
        return MapToDetailDto(created);
    }



    public async Task<UserDetailDto?> UpdateAsync(string dni, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByDNIAsync(dni);
        if (user == null) return null;

        var existingEmail = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingEmail != null && existingEmail.DNI != dni)
            throw new InvalidOperationException($"Ya existe otro usuario con email {dto.Email}");

        user.FirstName = dto.FirstName.Trim();
        user.LastName = dto.LastName.Trim();
        user.Email = dto.Email.Trim().ToLower();
        user.Role = dto.Role;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        return MapToDetailDto(user);
    }

    public async Task<bool> DeleteAsync(string dni)
    {
        var user = await _userRepository.GetByDNIAsync(dni);
        if (user == null) return false;

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
        return true;
    }

    private static string HashPassword(string password)
    {
        // Hash básico por ahora. En la Fase 4 usaremos ASP.NET Identity
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static UserListDto MapToListDto(User user)
    {
        return new UserListDto
        {
            DNI = user.DNI,
            FullName = $"{user.LastName}, {user.FirstName}",
            Email = user.Email,
            Role = user.Role
        };
    }

    private static UserDetailDto MapToDetailDto(User user)
    {
        return new UserDetailDto
        {
            DNI = user.DNI,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = $"{user.LastName}, {user.FirstName}",
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }
}