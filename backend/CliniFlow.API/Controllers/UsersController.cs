using CliniFlow.Application.DTOs;
using CliniFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CliniFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserListDto>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{dni}")]
    public async Task<ActionResult<UserDetailDto>> GetByDNI(string dni)
    {
        var user = await _userService.GetByDNIAsync(dni);
        if (user == null) return NotFound(new { message = "Usuario no encontrado" });
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDetailDto>> Create([FromBody] CreateUserDto dto)
    {
        try
        {
            var user = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByDNI), new { dni = user.DNI }, user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{dni}")]
    public async Task<ActionResult<UserDetailDto>> Update(string dni, [FromBody] UpdateUserDto dto)
    {
        try
        {
            var user = await _userService.UpdateAsync(dni, dto);
            if (user == null) return NotFound(new { message = "Usuario no encontrado" });
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{dni}")]
    public async Task<ActionResult> Delete(string dni)
    {
        var result = await _userService.DeleteAsync(dni);
        if (!result) return NotFound(new { message = "Usuario no encontrado" });
        return NoContent();
    }
}