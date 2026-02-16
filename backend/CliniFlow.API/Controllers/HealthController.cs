using Microsoft.AspNetCore.Mvc;

namespace CliniFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Status = "Healthy",
            Application = "CliniFlow API",
            Timestamp = DateTime.UtcNow
        });
    }
}