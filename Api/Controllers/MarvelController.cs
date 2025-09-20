using Microsoft.AspNetCore.Mvc;
using FrikiMarvelApi.Domain.DTOs;
using FrikiMarvelApi.Domain.Interfaces;
using FrikiMarvelApi.Domain.Models;

namespace FrikiMarvelApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarvelController : ControllerBase
{
    private readonly IMarvelApiService _marvelApiService;

    public MarvelController(IMarvelApiService marvelApiService)
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
    [HttpGet("characters")]
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
    [HttpGet("characters/{id}")]
    public async Task<ActionResult<ApiResponse<MarvelApiResponse<MarvelCharacter>>>> GetCharacterById(int id)
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
    [HttpGet("characters/search")]
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

    /// <summary>
    /// Obtiene cómics de Marvel con filtros opcionales
    /// </summary>
    /// <param name="title">Título exacto del cómic</param>
    /// <param name="titleStartsWith">Título que comience con</param>
    /// <param name="issueNumber">Número de edición</param>
    /// <param name="format">Formato del cómic (comic, magazine, trade paperback, etc.)</param>
    /// <param name="startYear">Año de inicio</param>
    /// <param name="endYear">Año de fin</param>
    /// <param name="limit">Número de resultados (máximo 100)</param>
    /// <param name="offset">Número de resultados a omitir</param>
    /// <param name="orderBy">Campo por el cual ordenar (title, issueNumber, modified, -title, -issueNumber, -modified)</param>
    /// <returns>Lista de cómics de Marvel</returns>
    [HttpGet("comics")]
    public async Task<ActionResult<ApiResponse<MarvelApiResponse<MarvelComic>>>> GetComics(
        [FromQuery] string? title = null,
        [FromQuery] string? titleStartsWith = null,
        [FromQuery] int? issueNumber = null,
        [FromQuery] string? format = null,
        [FromQuery] int? startYear = null,
        [FromQuery] int? endYear = null,
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0,
        [FromQuery] string orderBy = "title")
    {
        try
        {
            // Validar parámetros
            if (limit > 100) limit = 100;
            if (limit < 1) limit = 20;
            if (offset < 0) offset = 0;

            var request = new MarvelComicSearchRequest
            {
                Title = title,
                TitleStartsWith = titleStartsWith,
                IssueNumber = issueNumber,
                Format = format,
                StartYear = startYear,
                EndYear = endYear,
                Limit = limit,
                Offset = offset,
                OrderBy = orderBy
            };

            var result = await _marvelApiService.GetComicsAsync(request);
            
            return Ok(ApiResponse<MarvelApiResponse<MarvelComic>>.SuccessResponse(
                result, 
                $"Se obtuvieron {result.Data.Count} cómics de Marvel"));
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(502, ApiResponse<MarvelApiResponse<MarvelComic>>.ErrorResponse(
                ex.Message, 
                "Error al comunicarse con la API de Marvel"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MarvelApiResponse<MarvelComic>>.ErrorResponse(
                ex.Message, 
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Obtiene un cómic específico de Marvel por su ID
    /// </summary>
    /// <param name="id">ID del cómic en Marvel</param>
    /// <returns>Información del cómic</returns>
    [HttpGet("comics/{id}")]
    public async Task<ActionResult<ApiResponse<MarvelApiResponse<MarvelComic>>>> GetComicById(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponse<MarvelApiResponse<MarvelComic>>.ErrorResponse(
                    "ID debe ser un número positivo", 
                    "Parámetro inválido"));
            }

            var result = await _marvelApiService.GetComicByIdAsync(id);
            
            if (result.Data.Count == 0)
            {
                return NotFound(ApiResponse<MarvelApiResponse<MarvelComic>>.ErrorResponse(
                    $"Cómic con ID {id} no encontrado", 
                    "Cómic no encontrado"));
            }
            
            return Ok(ApiResponse<MarvelApiResponse<MarvelComic>>.SuccessResponse(
                result, 
                "Cómic obtenido exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(502, ApiResponse<MarvelApiResponse<MarvelComic>>.ErrorResponse(
                ex.Message, 
                "Error al comunicarse con la API de Marvel"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MarvelApiResponse<MarvelComic>>.ErrorResponse(
                ex.Message, 
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Busca cómics de Marvel por título
    /// </summary>
    /// <param name="title">Título del cómic a buscar</param>
    /// <param name="limit">Número de resultados (máximo 100)</param>
    /// <param name="offset">Número de resultados a omitir</param>
    /// <returns>Lista de cómics que coinciden con la búsqueda</returns>
    [HttpGet("comics/search")]
    public async Task<ActionResult<ApiResponse<MarvelApiResponse<MarvelComic>>>> SearchComics(
        [FromQuery] string title,
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest(ApiResponse<MarvelApiResponse<MarvelComic>>.ErrorResponse(
                    "El parámetro 'title' es requerido", 
                    "Parámetro requerido"));
            }

            // Validar parámetros
            if (limit > 100) limit = 100;
            if (limit < 1) limit = 20;
            if (offset < 0) offset = 0;

            var result = await _marvelApiService.SearchComicsByTitleAsync(title, limit, offset);
            
            return Ok(ApiResponse<MarvelApiResponse<MarvelComic>>.SuccessResponse(
                result, 
                $"Se encontraron {result.Data.Count} cómics que coinciden con '{title}'"));
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(502, ApiResponse<MarvelApiResponse<MarvelComic>>.ErrorResponse(
                ex.Message, 
                "Error al comunicarse con la API de Marvel"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<MarvelApiResponse<MarvelComic>>.ErrorResponse(
                ex.Message, 
                "Error interno del servidor"));
        }
    }
}
