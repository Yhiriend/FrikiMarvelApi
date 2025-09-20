using Microsoft.EntityFrameworkCore;
using FrikiMarvelApi.Domain.Entities;
using FrikiMarvelApi.Domain.Interfaces;

namespace FrikiMarvelApi.Infrastructure.Persistence.Repositories;

public class CharacterRepository : BaseRepository<Character>, ICharacterRepository
{
    public CharacterRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Character?> GetByMarvelIdAsync(string marvelId)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.MarvelId == marvelId && c.IsActive);
    }

    public async Task<IEnumerable<Character>> GetByNameAsync(string name)
    {
        return await _dbSet
            .Where(c => c.Name.Contains(name) && c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Character>> GetActiveCharactersAsync()
    {
        return await _dbSet
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}
