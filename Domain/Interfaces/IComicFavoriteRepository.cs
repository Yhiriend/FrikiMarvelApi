using FrikiMarvelApi.Domain.Entities;

namespace FrikiMarvelApi.Domain.Interfaces;

/// <summary>
/// Interfaz para el repositorio de cómics favoritos
/// </summary>
public interface IComicFavoriteRepository : IRepository<ComicFavorite>
{
    /// <summary>
    /// Obtiene todos los favoritos de un usuario
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Lista de favoritos del usuario</returns>
    Task<List<ComicFavorite>> GetByUserIdAsync(int userId);
    
    /// <summary>
    /// Verifica si un cómic ya está en favoritos del usuario
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="comicId">ID del cómic</param>
    /// <returns>True si ya está en favoritos</returns>
    Task<bool> ExistsByUserAndComicAsync(int userId, int comicId);
    
    /// <summary>
    /// Obtiene un favorito específico por usuario y cómic
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="comicId">ID del cómic</param>
    /// <returns>Favorito encontrado o null</returns>
    Task<ComicFavorite?> GetByUserAndComicAsync(int userId, int comicId);
    
    /// <summary>
    /// Elimina un favorito por usuario y cómic
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="comicId">ID del cómic</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> RemoveByUserAndComicAsync(int userId, int comicId);
    
    /// <summary>
    /// Guarda los cambios en la base de datos
    /// </summary>
    /// <returns>Task</returns>
    Task SaveChangesAsync();
}
