using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FrikiMarvelApi.Infrastructure.Middleware;

public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtAuthenticationMiddleware> _logger;

    // Rutas públicas que no requieren autenticación
    private static readonly string[] PublicRoutes = {
        "/api/auth/login",
        "/api/auth/register",
        "/health",
        "/swagger",
        "/openapi"
    };

    public JwtAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtAuthenticationMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();

        // Verificar si la ruta es pública
        if (IsPublicRoute(path))
        {
            await _next(context);
            return;
        }

        // Verificar si hay token en el header Authorization
        var token = ExtractTokenFromHeader(context.Request.Headers["Authorization"]);

        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("Token no encontrado para la ruta: {Path}", path);
            await WriteUnauthorizedResponse(context, "Token de autenticación requerido");
            return;
        }

        // Validar el token
        if (!ValidateToken(token))
        {
            _logger.LogWarning("Token inválido para la ruta: {Path}", path);
            await WriteUnauthorizedResponse(context, "Token de autenticación inválido");
            return;
        }

        // Token válido, continuar con la siguiente middleware
        await _next(context);
    }

    private bool IsPublicRoute(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        return PublicRoutes.Any(route => path.StartsWith(route.ToLower()));
    }

    private string? ExtractTokenFromHeader(string? authorizationHeader)
    {
        if (string.IsNullOrEmpty(authorizationHeader))
            return null;

        if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return null;

        return authorizationHeader.Substring("Bearer ".Length).Trim();
    }

    private bool ValidateToken(string token)
    {
        try
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"] ?? "FrikiMarvelApi",
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"] ?? "FrikiMarvelApiUsers",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar el token JWT");
            return false;
        }
    }

    private async Task WriteUnauthorizedResponse(HttpContext context, string message)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var response = new
        {
            success = false,
            message = message,
            data = (object?)null,
            error = "Unauthorized",
            timestamp = DateTime.UtcNow
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}
