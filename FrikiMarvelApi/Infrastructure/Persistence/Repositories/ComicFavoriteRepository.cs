using Microsoft.EntityFrameworkCore;
using FrikiMarvelApi.Domain.Entities;
using FrikiMarvelApi.Domain.Interfaces;
using FrikiMarvelApi.Infrastructure.Persistence;

namespace FrikiMarvelApi.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repositorio para cómics favoritos
/// </summary>
public class ComicFavoriteRepository : BaseRepository<ComicFavorite>, IComicFavoriteRepository
{
    public ComicFavoriteRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<ComicFavorite>> GetByUserIdAsync(int userId)
    {
        return await _context.ComicFavorites
            .Where(cf => cf.UserId == userId && cf.IsActive)
            .OrderByDescending(cf => cf.AddedDate)
            .ToListAsync();
    }

    public async Task<bool> ExistsByUserAndComicAsync(int userId, int comicId)
    {
        return await _context.ComicFavorites
            .AnyAsync(cf => cf.UserId == userId && cf.IsActive && cf.ComicData.Contains($"\"ComicId\":{comicId}"));
    }

    public async Task<ComicFavorite?> GetByUserAndComicAsync(int userId, int comicId)
    {
        return await _context.ComicFavorites
            .FirstOrDefaultAsync(cf => cf.UserId == userId && cf.IsActive && cf.ComicData.Contains($"\"ComicId\":{comicId}"));
    }

    public async Task<bool> RemoveByUserAndComicAsync(int userId, int comicId)
    {
        var favorite = await GetByUserAndComicAsync(userId, comicId);
        if (favorite == null)
            return false;

        favorite.IsActive = false;
        favorite.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
