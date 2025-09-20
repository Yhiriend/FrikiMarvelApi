using Microsoft.AspNetCore.Mvc;
using FrikiMarvelApi.Domain.Interfaces;

namespace FrikiMarvelApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;

    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    /// <summary>
    /// Verifica el estado de la API y la conexión a la base de datos
    /// </summary>
    /// <returns>Estado de salud de la aplicación</returns>
    [HttpGet]
    public async Task<IActionResult> GetHealth()
    {
        try
        {
            var healthStatus = await _healthService.CheckHealthAsync();
            
            var statusCode = healthStatus.IsHealthy ? 200 : 503;
            
            return StatusCode(statusCode, new
            {
                status = healthStatus.Status,
                isHealthy = healthStatus.IsHealthy,
                database = healthStatus.DatabaseStatus,
                checkedAt = healthStatus.CheckedAt,
                details = healthStatus.Details,
                message = healthStatus.IsHealthy 
                    ? "API funcionando correctamente" 
                    : "API con problemas de conectividad"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "Error",
                isHealthy = false,
                database = "Unknown",
                checkedAt = DateTime.UtcNow,
                message = "Error interno del servidor",
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Endpoint simple para verificar que la API está funcionando
    /// </summary>
    /// <returns>Mensaje de estado básico</returns>
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(new
        {
            message = "Pong! API funcionando",
            timestamp = DateTime.UtcNow,
            version = "1.0.0"
        });
    }
}
