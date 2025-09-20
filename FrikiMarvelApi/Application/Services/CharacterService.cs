using FrikiMarvelApi.Domain.Entities;
using FrikiMarvelApi.Domain.Interfaces;

namespace FrikiMarvelApi.Application.Services;

public class CharacterService
{
    private readonly ICharacterRepository _characterRepository;

    public CharacterService(ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<Character?> GetCharacterByIdAsync(int id)
    {
        return await _characterRepository.GetByIdAsync(id);
    }

    public async Task<Character?> GetCharacterByMarvelIdAsync(string marvelId)
    {
        return await _characterRepository.GetByMarvelIdAsync(marvelId);
    }

    public async Task<IEnumerable<Character>> GetAllCharactersAsync()
    {
        return await _characterRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Character>> GetActiveCharactersAsync()
    {
        return await _characterRepository.GetActiveCharactersAsync();
    }

    public async Task<IEnumerable<Character>> SearchCharactersByNameAsync(string name)
    {
        return await _characterRepository.GetByNameAsync(name);
    }

    public async Task<Character> CreateCharacterAsync(Character character)
    {
        character.CreatedAt = DateTime.UtcNow;
        character.IsActive = true;
        return await _characterRepository.AddAsync(character);
    }

    public async Task<Character> UpdateCharacterAsync(Character character)
    {
        character.UpdatedAt = DateTime.UtcNow;
        await _characterRepository.UpdateAsync(character);
        return character;
    }

    public async Task DeleteCharacterAsync(int id)
    {
        await _characterRepository.DeleteAsync(id);
    }
}
