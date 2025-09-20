using FrikiMarvelApi.Domain.DTOs;

namespace FrikiMarvelApi.Domain.Interfaces;

public interface IMarvelApiService
{
    // Métodos de personajes
    Task<MarvelApiResponse<MarvelCharacter>> GetCharactersAsync(MarvelCharacterSearchRequest? request = null);
    Task<MarvelApiResponse<MarvelCharacter>> GetCharacterByIdAsync(int characterId);
    Task<MarvelApiResponse<MarvelCharacter>> SearchCharactersByNameAsync(string name, int limit = 20, int offset = 0);
    
    // Métodos de cómics
    Task<MarvelApiResponse<MarvelComic>> GetComicsAsync(MarvelComicSearchRequest? request = null);
    Task<MarvelApiResponse<MarvelComic>> GetComicByIdAsync(int comicId);
    Task<MarvelApiResponse<MarvelComic>> SearchComicsByTitleAsync(string title, int limit = 20, int offset = 0);
}
