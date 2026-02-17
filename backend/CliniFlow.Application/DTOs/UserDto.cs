using System;
using System.Collections.Generic;
using System.Text;

using CliniFlow.Domain.Enums;

namespace CliniFlow.Application.DTOs;

// Para la LISTA de usuarios
public class UserListDto
{
    public string DNI { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
}

// Para el DETALLE de un usuario
public class UserDetailDto
{
    public string DNI { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Para CREAR un usuario
public class CreateUserDto
{
    public string DNI { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Role Role { get; set; }
}

// Para ACTUALIZAR un usuario (sin cambiar DNI ni password)
public class UpdateUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
}