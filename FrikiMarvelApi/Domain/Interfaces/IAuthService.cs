using FrikiMarvelApi.Domain.DTOs;

namespace FrikiMarvelApi.Domain.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<bool> ValidateTokenAsync(string token);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
}
