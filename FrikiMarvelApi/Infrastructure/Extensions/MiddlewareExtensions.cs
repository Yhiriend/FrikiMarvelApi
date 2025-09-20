using FrikiMarvelApi.Infrastructure.Middleware;

namespace FrikiMarvelApi.Infrastructure.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtAuthenticationMiddleware>();
    }
}
