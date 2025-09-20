using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FrikiMarvelApi.Domain.DTOs;
using FrikiMarvelApi.Domain.Interfaces;
using FrikiMarvelApi.Domain.Models;

namespace FrikiMarvelApi.Api.Controllers;

/// <summary>
/// Controlador para manejar cómics favoritos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Requiere autenticación
public class ComicFavoritesController : ControllerBase
{
    private readonly IComicFavoriteService _comicFavoriteService;
    private readonly ILogger<ComicFavoritesController> _logger;

    public ComicFavoritesController(
        IComicFavoriteService comicFavoriteService,
        ILogger<ComicFavoritesController> logger)
    {
        _comicFavoriteService = comicFavoriteService;
        _logger = logger;
    }

    /// <summary>
    /// Agrega un cómic a favoritos
    /// </summary>
    /// <param name="request">Datos del cómic a agregar</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> AddToFavorites([FromBody] AddComicFavoriteRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse.ErrorResponse(
                    "Usuario no autenticado", 
                    "Token de autenticación inválido"));
            }

            var result = await _comicFavoriteService.AddToFavoritesAsync(userId.Value, request);
            
            if (!result)
            {
                return BadRequest(ApiResponse.ErrorResponse(
                    "No se pudo agregar el cómic a favoritos. Verifique que el cómic no esté ya en favoritos y que el usuario exista.", 
                    "Error al agregar favorito"));
            }

            return Ok(ApiResponse.SuccessResponse(
                "Cómic agregado a favoritos exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar cómic a favoritos");
            return StatusCode(500, ApiResponse.ErrorResponse(
                ex.Message, 
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Obtiene todos los cómics favoritos del usuario autenticado
    /// </summary>
    /// <returns>Lista de cómics favoritos</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<ComicFavoritesResponse>>> GetFavorites()
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<ComicFavoritesResponse>.ErrorResponse(
                    "Usuario no autenticado", 
                    "Token de autenticación inválido"));
            }

            var favorites = await _comicFavoriteService.GetUserFavoritesAsync(userId.Value);
            
            return Ok(ApiResponse<ComicFavoritesResponse>.SuccessResponse(
                favorites, 
                $"Se encontraron {favorites.TotalCount} cómics favoritos"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cómics favoritos");
            return StatusCode(500, ApiResponse<ComicFavoritesResponse>.ErrorResponse(
                ex.Message, 
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Elimina un cómic de favoritos
    /// </summary>
    /// <param name="comicId">ID del cómic a eliminar</param>
    /// <returns>Resultado de la operación</returns>
    [HttpDelete("{comicId}")]
    public async Task<ActionResult<ApiResponse>> RemoveFromFavorites(int comicId)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse.ErrorResponse(
                    "Usuario no autenticado", 
                    "Token de autenticación inválido"));
            }

            if (comicId <= 0)
            {
                return BadRequest(ApiResponse.ErrorResponse(
                    "ID del cómic debe ser un número positivo", 
                    "Parámetro inválido"));
            }

            var result = await _comicFavoriteService.RemoveFromFavoritesAsync(userId.Value, comicId);
            
            if (!result)
            {
                return NotFound(ApiResponse.ErrorResponse(
                    $"Cómic con ID {comicId} no encontrado en favoritos", 
                    "Favorito no encontrado"));
            }

            return Ok(ApiResponse.SuccessResponse(
                "Cómic eliminado de favoritos exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar cómic de favoritos");
            return StatusCode(500, ApiResponse.ErrorResponse(
                ex.Message, 
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Verifica si un cómic está en favoritos
    /// </summary>
    /// <param name="comicId">ID del cómic a verificar</param>
    /// <returns>True si está en favoritos</returns>
    [HttpGet("{comicId}/check")]
    public async Task<ActionResult<ApiResponse<bool>>> CheckIfFavorite(int comicId)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<bool>.ErrorResponse(
                    "Usuario no autenticado", 
                    "Token de autenticación inválido"));
            }

            if (comicId <= 0)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse(
                    "ID del cómic debe ser un número positivo", 
                    "Parámetro inválido"));
            }

            var isFavorite = await _comicFavoriteService.IsFavoriteAsync(userId.Value, comicId);
            
            return Ok(ApiResponse<bool>.SuccessResponse(
                isFavorite, 
                isFavorite ? "El cómic está en favoritos" : "El cómic no está en favoritos"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar si el cómic está en favoritos");
            return StatusCode(500, ApiResponse<bool>.ErrorResponse(
                ex.Message, 
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Obtiene el ID del usuario actual desde el token JWT
    /// </summary>
    /// <returns>ID del usuario o null si no está autenticado</returns>
    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out int userId))
        {
            return userId;
        }
        return null;
    }
}
