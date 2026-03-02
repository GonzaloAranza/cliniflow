using CliniFlow.Application.DTOs;
using CliniFlow.Application.Interfaces;
using CliniFlow.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CliniFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository; // O IUserService si la lógica está ahí
    private readonly IJwtProvider _jwtProvider;

    public AuthController(IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        // 1. Buscamos al usuario por su email
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            return Unauthorized(new { message = "Credenciales inválidas." });
        }

        // 2. Verificamos la contraseña
        // IMPORTANTE: Acá deberías usar la misma librería con la que hasheaste la password 
        // al crear el usuario (por ejemplo, BCrypt.Net-Next).
        bool isPasswordValid = VerifyPassword(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Unauthorized(new { message = "Credenciales inválidas." });
        }

        // 3. Si todo está ok, generamos el JWT
        var token = _jwtProvider.Generate(user);

        // 4. Devolvemos el token al cliente
        return Ok(new { token });
    }

    // Método de ejemplo (reemplazar por tu lógica real de validación de hashes)
    static bool VerifyPassword(string plainPassword, string hashedPassword)
    {
         
         return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);

    }
}