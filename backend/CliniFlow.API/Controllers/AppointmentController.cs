using CliniFlow.Application.DTOs;
using CliniFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CliniFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    // POST: api/appointments
    // Agendar un nuevo turno
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateAppointmentDto dto)
    {
        try
        {
            var id = await _appointmentService.CreateAppointmentAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = id }, new { id = id });
        }
        catch (InvalidOperationException ex)
        {
            // Captura errores de negocio: "Horario ocupado" o "Fecha pasada"
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            // Captura si el Paciente o Profesional no existen
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Cambiamos esto para ver el error real (InnerException)
            return StatusCode(500, new
            {
                message = "Error interno.",
                error = ex.Message,
                inner = ex.InnerException?.Message, // <--- Esto es lo que necesitamos ver
                stackTrace = ex.StackTrace
            });
        }
    }

    // GET: api/appointments/5
    // Ver detalle de un turno
    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDetailDto>> GetById(int id)
    {
        var appointment = await _appointmentService.GetByIdAsync(id);
        if (appointment == null) return NotFound($"No se encontró el turno {id}");

        return Ok(appointment);
    }

    // GET: api/appointments/professional/1?date=2026-02-20
    // Ver la agenda de un médico (Opcional: filtrar por día)
    [HttpGet("professional/{professionalId}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByProfessional(int professionalId, [FromQuery] DateOnly? date)
    {
        var appointments = await _appointmentService.GetByProfessionalAsync(professionalId, date);
        return Ok(appointments);
    }

    // POST: api/appointments/5/cancel
    // Cancelar un turno (Usamos POST porque es una acción, no un borrado físico)
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id, [FromBody] CancelAppointmentRequest request)
    {
        try
        {
            await _appointmentService.CancelAppointmentAsync(id, request.Reason);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"No se encontró el turno {id}");
        }
        catch (InvalidOperationException ex)
        {
            // Ej: Intentar cancelar un turno que ya pasó o ya estaba cancelado
            return BadRequest(new { message = ex.Message });
        }
    }
}

// DTO pequeño solo para este controller (para recibir el motivo de cancelación)
public class CancelAppointmentRequest
{
    public string Reason { get; set; } = string.Empty;
}