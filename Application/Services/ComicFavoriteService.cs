using System.Text.Json;
using FrikiMarvelApi.Domain.DTOs;
using FrikiMarvelApi.Domain.Entities;
using FrikiMarvelApi.Domain.Interfaces;

namespace FrikiMarvelApi.Application.Services;

/// <summary>
/// Servicio para manejar cómics favoritos
/// </summary>
public class ComicFavoriteService : IComicFavoriteService
{
    private readonly IComicFavoriteRepository _comicFavoriteRepository;
    private readonly IUserRepository _userRepository;
    private readonly JsonSerializerOptions _jsonOptions;

    public ComicFavoriteService(
        IComicFavoriteRepository comicFavoriteRepository,
        IUserRepository userRepository)
    {
        _comicFavoriteRepository = comicFavoriteRepository;
        _userRepository = userRepository;
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<bool> AddToFavoritesAsync(int userId, AddComicFavoriteRequest request)
    {
        // Verificar que el usuario existe
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        // Verificar si ya está en favoritos
        var exists = await _comicFavoriteRepository.ExistsByUserAndComicAsync(userId, request.ComicId);
        if (exists)
            return false; // Ya está en favoritos

        // Crear el DTO del cómic
        var comicDto = new ComicFavoriteDto
        {
            ComicId = request.ComicId,
            ImageUrl = request.ImageUrl,
            Format = request.Format,
            Title = request.Title,
            OnSaleDate = request.OnSaleDate,
            Author = request.Author,
            Price = request.Price,
            Characters = request.Characters,
            AddedDate = DateTime.UtcNow
        };

        // Serializar a JSON
        var comicJson = JsonSerializer.Serialize(comicDto, _jsonOptions);

        // Crear la entidad
        var favorite = new ComicFavorite
        {
            UserId = userId,
            ComicData = comicJson,
            AddedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Guardar en la base de datos
        await _comicFavoriteRepository.AddAsync(favorite);
        await _comicFavoriteRepository.SaveChangesAsync();

        return true;
    }

    public async Task<ComicFavoritesResponse> GetUserFavoritesAsync(int userId)
    {
        var favorites = await _comicFavoriteRepository.GetByUserIdAsync(userId);
        
        var comicDtos = new List<ComicFavoriteDto>();
        
        foreach (var favorite in favorites)
        {
            try
            {
                var comicDto = JsonSerializer.Deserialize<ComicFavoriteDto>(favorite.ComicData, _jsonOptions);
                if (comicDto != null)
                {
                    comicDtos.Add(comicDto);
                }
            }
            catch (JsonException)
            {
                // Si hay un error de deserialización, continuar con el siguiente
                continue;
            }
        }

        return new ComicFavoritesResponse
        {
            Favorites = comicDtos,
            TotalCount = comicDtos.Count
        };
    }

    public async Task<bool> RemoveFromFavoritesAsync(int userId, int comicId)
    {
        return await _comicFavoriteRepository.RemoveByUserAndComicAsync(userId, comicId);
    }

    public async Task<bool> IsFavoriteAsync(int userId, int comicId)
    {
        return await _comicFavoriteRepository.ExistsByUserAndComicAsync(userId, comicId);
    }
}
