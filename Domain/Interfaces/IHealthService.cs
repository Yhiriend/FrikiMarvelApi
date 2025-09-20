namespace FrikiMarvelApi.Domain.Interfaces;

public interface IHealthService
{
    Task<HealthStatus> CheckHealthAsync();
}

public class HealthStatus
{
    public bool IsHealthy { get; set; }
    public string Status { get; set; } = string.Empty;
    public string DatabaseStatus { get; set; } = string.Empty;
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    public Dictionary<string, object> Details { get; set; } = new();
}
