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
    public async Task<ActionResult<IEnumerable<PatientListDto>>> GetAll()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients);
    }

    [HttpGet("{dni}")]
    public async Task<ActionResult<PatientDetailDto>> GetByDNI(string dni)
    {
        var patient = await _patientService.GetByDNIAsync(dni);
        if (patient == null) return NotFound(new { message = "Paciente no encontrado" });
        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientDetailDto>> Create([FromBody] CreatePatientDto dto)
    {
        try
        {
            var patient = await _patientService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByDNI), new { dni = patient.DNI }, patient);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{dni}")]
    public async Task<ActionResult<PatientDetailDto>> Update(string dni, [FromBody] UpdatePatientDto dto)
    {
        try
        {
            var patient = await _patientService.UpdateAsync(dni, dto);
            if (patient == null) return NotFound(new { message = "Paciente no encontrado" });
            return Ok(patient);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{dni}")]
    public async Task<ActionResult> Delete(string dni)
    {
        var result = await _patientService.DeleteAsync(dni);
        if (!result) return NotFound(new { message = "Paciente no encontrado" });
        return NoContent();
    }
}