using FrikiMarvelApi.Domain.Interfaces;
using FrikiMarvelApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrikiMarvelApi.Application.Services;

public class HealthService : IHealthService
{
    private readonly AppDbContext _context;

    public HealthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<HealthStatus> CheckHealthAsync()
    {
        var healthStatus = new HealthStatus
        {
            CheckedAt = DateTime.UtcNow
        };

        try
        {
            // Verificar conexión a la base de datos
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (canConnect)
            {
                // Verificar que las tablas existen ejecutando una consulta simple
                await _context.Database.ExecuteSqlRawAsync("SELECT 1");
                
                healthStatus.DatabaseStatus = "Connected";
                healthStatus.IsHealthy = true;
                healthStatus.Status = "Healthy";
                healthStatus.Details.Add("Database", "Connected and accessible");
            }
            else
            {
                healthStatus.DatabaseStatus = "Disconnected";
                healthStatus.IsHealthy = false;
                healthStatus.Status = "Unhealthy";
                healthStatus.Details.Add("Database", "Cannot connect to database");
            }
        }
        catch (Exception ex)
        {
            healthStatus.DatabaseStatus = "Error";
            healthStatus.IsHealthy = false;
            healthStatus.Status = "Unhealthy";
            healthStatus.Details.Add("Database", $"Error: {ex.Message}");
        }

        // Información adicional del sistema
        healthStatus.Details.Add("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown");
        healthStatus.Details.Add("MachineName", Environment.MachineName);
        healthStatus.Details.Add("OSVersion", Environment.OSVersion.ToString());

        return healthStatus;
    }
}
