using FrikiMarvelApi.Domain.DTOs;

namespace FrikiMarvelApi.Domain.Interfaces;

/// <summary>
/// Interfaz para el servicio de cómics favoritos
/// </summary>
public interface IComicFavoriteService
{
    /// <summary>
    /// Agrega un cómic a favoritos
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="request">Datos del cómic a agregar</param>
    /// <returns>True si se agregó correctamente</returns>
    Task<bool> AddToFavoritesAsync(int userId, AddComicFavoriteRequest request);
    
    /// <summary>
    /// Obtiene todos los favoritos de un usuario
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Lista de favoritos del usuario</returns>
    Task<ComicFavoritesResponse> GetUserFavoritesAsync(int userId);
    
    /// <summary>
    /// Elimina un cómic de favoritos
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="comicId">ID del cómic</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> RemoveFromFavoritesAsync(int userId, int comicId);
    
    /// <summary>
    /// Verifica si un cómic está en favoritos
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="comicId">ID del cómic</param>
    /// <returns>True si está en favoritos</returns>
    Task<bool> IsFavoriteAsync(int userId, int comicId);
}
