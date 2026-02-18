using CliniFlow.Application.DTOs;
using CliniFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CliniFlow.API.Controllers;

[Route("api/medical-records")]
[ApiController]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMedicalRecordService _service;

    public MedicalRecordsController(IMedicalRecordService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMedicalRecordDto dto)
    {
        try
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetHistory), new { patientId = 0 }, new { id });
            // Nota: El CreatedAtAction es simbólico aquí porque no tenemos un GetById directo expuesto aún
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetHistory(int patientId)
    {
        var history = await _service.GetHistoryByPatientAsync(patientId);
        return Ok(history);
    }
}