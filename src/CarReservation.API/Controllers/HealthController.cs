namespace CarReservation.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarReservation.Infrastructure.Persistence;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly CarReservationDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(CarReservationDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            // Check database connection
            await _context.Database.CanConnectAsync();
            
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Services = new
                {
                    Database = "Connected"
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            
            return StatusCode(503, new
            {
                Status = "Unhealthy",
                Timestamp = DateTime.UtcNow,
                Error = ex.Message
            });
        }
    }
}
