using FrikiMarvelApi.Domain.Entities;

namespace FrikiMarvelApi.Domain.Interfaces;

public interface ICharacterRepository : IRepository<Character>
{
    Task<Character?> GetByMarvelIdAsync(string marvelId);
    Task<IEnumerable<Character>> GetByNameAsync(string name);
    Task<IEnumerable<Character>> GetActiveCharactersAsync();
}
