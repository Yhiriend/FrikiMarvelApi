using Microsoft.AspNetCore.Mvc;
using FrikiMarvelApi.Domain.DTOs;
using FrikiMarvelApi.Domain.Interfaces;
using FrikiMarvelApi.Domain.Models;

namespace FrikiMarvelApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharactersController : ControllerBase
{
    private readonly IMarvelApiService _marvelApiService;

    public CharactersController(IMarvelApiService marvelApiService)
    {
        _marvelApiService = marvelApiService;
    }

    /// <summary>
    /// Obtiene personajes de Marvel con filtros opcionales
    /// </summary>
    /// <param name="name">Nombre exacto del personaje</param>
    /// <param name="nameStartsWith">Nombre que comience con</param>
    /// <param name="limit">Número de resultados (máximo 100)</param>
    /// <param name="offset">Número de resultados a omitir</param>
    /// <param name="orderBy">Campo por el cual ordenar (name, modified, -name, -modified)</param>
    /// <returns>Lista de personajes de Marvel</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<MarvelApiResponse<MarvelCharacter>>>> GetCharacters(
        [FromQuery] string? name = null,
        [FromQuery] string? nameStartsWith = null,
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0,
        [FromQuery] string orderBy = "name")
    {
        try
        {
            // Validar parámetros
            if (limit > 100) limit = 100;
            if (limit < 1) limit = 20;
            if (offset < 0) offset = 0;

            var request = new MarvelCharacterSearchRequest
            {
                Name = name,
                NameStartsWith = nameStartsWith,
                Limit = limit,
                Offset = offset,
                OrderBy = orderBy
            };

            var result = await _marvelApiService.GetCharactersAsync(request);
            
            return Ok(ApiResponse<MarvelApiResponse<MarvelCharacter>>.SuccessResponse(
                result, 
                $"Se obtuvieron {result.Data.Count} personajes de Marvel"));
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(502, ApiResponse<MarvelApiResponse<MarvelCharacter>>.ErrorResponse(
                ex.Message, 
                "Error al comunicarse con la API de Marvel"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MarvelApiResponse<MarvelCharacter>>.ErrorResponse(
                ex.Message, 
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Obtiene un personaje específico de Marvel por su ID
    /// </summary>
    /// <param name="id">ID del personaje en Marvel</param>
    /// <returns>Información del personaje</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<MarvelApiResponse<MarvelCharacter>>>> GetCharacter(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponse<MarvelApiResponse<MarvelCharacter>>.ErrorResponse(
                    "ID debe ser un número positivo", 
                    "Parámetro inválido"));
            }

            var result = await _marvelApiService.GetCharacterByIdAsync(id);
            
            if (result.Data.Count == 0)
            {
                return NotFound(ApiResponse<MarvelApiResponse<MarvelCharacter>>.ErrorResponse(
                    $"Personaje con ID {id} no encontrado", 
                    "Personaje no encontrado"));
            }
            
            return Ok(ApiResponse<MarvelApiResponse<MarvelCharacter>>.SuccessResponse(
                result, 
                "Personaje obtenido exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(502, ApiResponse<MarvelApiResponse<MarvelCharacter>>.ErrorResponse(
                ex.Message, 
                "Error al comunicarse con la API de Marvel"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MarvelApiResponse<MarvelCharacter>>.ErrorResponse(
                ex.Message, 
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Busca personajes de Marvel por nombre
    /// </summary>
    /// <param name="name">Nombre del personaje a buscar</param>
    /// <param name="limit">Número de resultados (máximo 100)</param>
    /// <param name="offset">Número de resultados a omitir</param>
    /// <returns>Lista de personajes que coinciden con la búsqueda</returns>
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<MarvelApiResponse<MarvelCharacter>>>> SearchCharacters(
        [FromQuery] string name,
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(ApiResponse<MarvelApiResponse<MarvelCharacter>>.ErrorResponse(
                    "El parámetro 'name' es requerido", 
                    "Parámetro requerido"));
            }

            // Validar parámetros
            if (limit > 100) limit = 100;
            if (limit < 1) limit = 20;
            if (offset < 0) offset = 0;

            var result = await _marvelApiService.SearchCharactersByNameAsync(name, limit, offset);
            
            return Ok(ApiResponse<MarvelApiResponse<MarvelCharacter>>.SuccessResponse(
                result, 
                $"Se encontraron {result.Data.Count} personajes que coinciden con '{name}'"));
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(502, ApiResponse<MarvelApiResponse<MarvelCharacter>>.ErrorResponse(
                ex.Message, 
                "Error al comunicarse con la API de Marvel"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MarvelApiResponse<MarvelCharacter>>.ErrorResponse(
                ex.Message, 
                "Error interno del servidor"));
        }
    }

}
