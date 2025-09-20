using Microsoft.AspNetCore.Mvc;
using FrikiMarvelApi.Domain.DTOs;
using FrikiMarvelApi.Domain.Interfaces;
using FrikiMarvelApi.Domain.Models;

namespace FrikiMarvelApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema
    /// </summary>
    /// <param name="request">Datos del usuario a registrar</param>
    /// <returns>Token de autenticación y datos del usuario</returns>
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(ApiResponse<AuthResponse>.ErrorResponse(
                    string.Join(", ", errors), 
                    "Validation failed"));
            }

            var result = await _authService.RegisterAsync(request);
            
            return Ok(ApiResponse<AuthResponse>.SuccessResponse(
                result, 
                "User registered successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<AuthResponse>.ErrorResponse(
                ex.Message, 
                "Registration failed"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<AuthResponse>.ErrorResponse(
                ex.Message, 
                "Internal server error"));
        }
    }

    /// <summary>
    /// Autentica un usuario existente
    /// </summary>
    /// <param name="request">Credenciales de login</param>
    /// <returns>Token de autenticación y datos del usuario</returns>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(ApiResponse<AuthResponse>.ErrorResponse(
                    string.Join(", ", errors), 
                    "Validation failed"));
            }

            var result = await _authService.LoginAsync(request);
            
            return Ok(ApiResponse<AuthResponse>.SuccessResponse(
                result, 
                "Login successful"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<AuthResponse>.ErrorResponse(
                ex.Message, 
                "Authentication failed"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<AuthResponse>.ErrorResponse(
                ex.Message, 
                "Internal server error"));
        }
    }

    /// <summary>
    /// Valida un token JWT
    /// </summary>
    /// <param name="token">Token a validar</param>
    /// <returns>Resultado de la validación</returns>
    [HttpPost("validate-token")]
    public async Task<ActionResult<ApiResponse<bool>>> ValidateToken([FromBody] string token)
    {
        try
        {
            var isValid = await _authService.ValidateTokenAsync(token);
            
            return Ok(ApiResponse<bool>.SuccessResponse(
                isValid, 
                isValid ? "Token is valid" : "Token is invalid"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResponse(
                ex.Message, 
                "Token validation failed"));
        }
    }
}
