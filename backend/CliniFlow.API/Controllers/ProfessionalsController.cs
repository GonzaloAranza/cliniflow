using CliniFlow.Application.DTOs;
using CliniFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CliniFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfessionalsController : ControllerBase
{
    private readonly IProfessionalService _professionalService;

    public ProfessionalsController(IProfessionalService professionalService)
    {
        _professionalService = professionalService;
    }

    // GET: api/professionals
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProfessionalListDto>>> GetAll()
    {
        var professionals = await _professionalService.GetAllAsync();
        return Ok(professionals);
    }

    // GET: api/professionals/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProfessionalDetailDto>> GetById(int id)
    {
        var professional = await _professionalService.GetByIdAsync(id);

        if (professional == null)
        {
            return NotFound($"No se encontró el profesional con ID {id}");
        }

        return Ok(professional);
    }

    // POST: api/professionals
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateProfessionalDto dto)
    {
        try
        {
            var id = await _professionalService.RegisterProfessionalAsync(dto);

            // Retorna un 201 Created y la URL para consultar el nuevo recurso
            return CreatedAtAction(nameof(GetById), new { id = id }, new { id = id });
        }
        catch (InvalidOperationException ex)
        {
            // Capturamos validaciones de negocio (Email duplicado, Matrícula existente)
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Error inesperado
            return StatusCode(500, new { message = "Ocurrió un error interno.", details = ex.Message });
        }
    }

    // PUT: api/professionals/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProfessionalDto dto)
    {
        try
        {
            await _professionalService.UpdateAsync(id, dto);
            return NoContent(); // 204: Todo salió bien, no hay contenido que devolver
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"No se encontró el profesional con ID {id}");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/professionals/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _professionalService.DeleteAsync(id);
        return NoContent();
    }
}