using CliniFlow.Application.DTOs;
using CliniFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CliniFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> GetById(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        if (patient == null) return NotFound(new { message = "Paciente no encontrado" });
        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> Create([FromBody] CreatePatientDto dto)
    {
        try
        {
            var patient = await _patientService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PatientDto>> Update(int id, [FromBody] UpdatePatientDto dto)
    {
        try
        {
            var patient = await _patientService.UpdateAsync(id, dto);
            if (patient == null) return NotFound(new { message = "Paciente no encontrado" });
            return Ok(patient);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _patientService.DeleteAsync(id);
        if (!result) return NotFound(new { message = "Paciente no encontrado" });
        return NoContent();
    }
}