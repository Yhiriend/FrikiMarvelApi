using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FrikiMarvelApi.Domain.DTOs;
using FrikiMarvelApi.Domain.Interfaces;

namespace FrikiMarvelApi.Application.Services;

public class MarvelApiService : IMarvelApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MarvelApiService> _logger;
    private readonly string _baseUrl;
    private readonly string _publicKey;
    private readonly string _privateKey;
    private readonly JsonSerializerOptions _jsonOptions;

    public MarvelApiService(HttpClient httpClient, IConfiguration configuration, ILogger<MarvelApiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;

        var marvelConfig = _configuration.GetSection("MarvelApi");
        _baseUrl = marvelConfig["BaseUrl"] ?? "https://gateway.marvel.com/v1/public";
        _publicKey = marvelConfig["PublicKey"] ?? "";
        _privateKey = marvelConfig["PrivateKey"] ?? "";

        var timeoutSeconds = marvelConfig.GetValue<int>("TimeoutSeconds", 30);
        _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

        if (string.IsNullOrEmpty(_publicKey) || string.IsNullOrEmpty(_privateKey))
        {
            _logger.LogWarning("Marvel API keys are not configured. Please set MarvelApi:PublicKey and MarvelApi:PrivateKey in appsettings.json");
        }

        // Configurar opciones de JSON
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<MarvelApiResponse<MarvelCharacter>> GetCharactersAsync(MarvelCharacterSearchRequest? request = null)
    {
        try
        {
            var queryParams = BuildQueryParameters(request);
            var url = $"{_baseUrl}/characters{queryParams}";
            
            _logger.LogInformation("Fetching characters from Marvel API: {Url}", url);
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var jsonContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MarvelApiResponse<MarvelCharacter>>(jsonContent, _jsonOptions);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to deserialize Marvel API response");
            }

            _logger.LogInformation("Successfully fetched {Count} characters from Marvel API", result.Data.Count);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error occurred while fetching characters from Marvel API");
            throw new InvalidOperationException("Error communicating with Marvel API", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error occurred while processing Marvel API response");
            throw new InvalidOperationException("Error processing Marvel API response", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while fetching characters from Marvel API");
            throw;
        }
    }

    public async Task<MarvelApiResponse<MarvelCharacter>> GetCharacterByIdAsync(int characterId)
    {
        try
        {
            var queryParams = BuildQueryParameters();
            var url = $"{_baseUrl}/characters/{characterId}{queryParams}";
            
            _logger.LogInformation("Fetching character {CharacterId} from Marvel API: {Url}", characterId, url);
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var jsonContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MarvelApiResponse<MarvelCharacter>>(jsonContent, _jsonOptions);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to deserialize Marvel API response");
            }

            _logger.LogInformation("Successfully fetched character {CharacterId} from Marvel API", characterId);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error occurred while fetching character {CharacterId} from Marvel API", characterId);
            throw new InvalidOperationException($"Error communicating with Marvel API for character {characterId}", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error occurred while processing Marvel API response for character {CharacterId}", characterId);
            throw new InvalidOperationException($"Error processing Marvel API response for character {characterId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while fetching character {CharacterId} from Marvel API", characterId);
            throw;
        }
    }

    public async Task<MarvelApiResponse<MarvelCharacter>> SearchCharactersByNameAsync(string name, int limit = 20, int offset = 0)
    {
        var request = new MarvelCharacterSearchRequest
        {
            Name = name,
            Limit = limit,
            Offset = offset
        };

        return await GetCharactersAsync(request);
    }

    public async Task<MarvelApiResponse<MarvelComic>> GetComicsAsync(MarvelComicSearchRequest? request = null)
    {
        try
        {
            var queryParams = BuildComicQueryParameters(request);
            var url = $"{_baseUrl}/comics{queryParams}";
            
            _logger.LogInformation("Fetching comics from Marvel API: {Url}", url);
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var jsonContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MarvelApiResponse<MarvelComic>>(jsonContent, _jsonOptions);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to deserialize Marvel API response");
            }

            _logger.LogInformation("Successfully fetched {Count} comics from Marvel API", result.Data.Count);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error occurred while fetching comics from Marvel API");
            throw new InvalidOperationException("Error communicating with Marvel API", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error occurred while processing Marvel API response");
            throw new InvalidOperationException("Error processing Marvel API response", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while fetching comics from Marvel API");
            throw;
        }
    }

    public async Task<MarvelApiResponse<MarvelComic>> GetComicByIdAsync(int comicId)
    {
        try
        {
            var queryParams = BuildComicQueryParameters();
            var url = $"{_baseUrl}/comics/{comicId}{queryParams}";
            
            _logger.LogInformation("Fetching comic {ComicId} from Marvel API: {Url}", comicId, url);
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var jsonContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MarvelApiResponse<MarvelComic>>(jsonContent, _jsonOptions);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to deserialize Marvel API response");
            }

            _logger.LogInformation("Successfully fetched comic {ComicId} from Marvel API", comicId);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error occurred while fetching comic {ComicId} from Marvel API", comicId);
            throw new InvalidOperationException($"Error communicating with Marvel API for comic {comicId}", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error occurred while processing Marvel API response for comic {ComicId}", comicId);
            throw new InvalidOperationException($"Error processing Marvel API response for comic {comicId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while fetching comic {ComicId} from Marvel API", comicId);
            throw;
        }
    }

    public async Task<MarvelApiResponse<MarvelComic>> SearchComicsByTitleAsync(string title, int limit = 20, int offset = 0)
    {
        var request = new MarvelComicSearchRequest
        {
            Title = title,
            Limit = limit,
            Offset = offset
        };

        return await GetComicsAsync(request);
    }

    private string BuildQueryParameters(MarvelCharacterSearchRequest? request = null)
    {
        var parameters = new List<string>();

        // Parámetros de autenticación requeridos por Marvel API
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var hash = GenerateMarvelHash(timestamp);

        parameters.Add($"ts={timestamp}");
        parameters.Add($"apikey={_publicKey}");
        parameters.Add($"hash={hash}");

        // Parámetros de búsqueda opcionales
        if (request != null)
        {
            if (!string.IsNullOrEmpty(request.Name))
                parameters.Add($"name={Uri.EscapeDataString(request.Name)}");
            
            if (!string.IsNullOrEmpty(request.NameStartsWith))
                parameters.Add($"nameStartsWith={Uri.EscapeDataString(request.NameStartsWith)}");
            
            if (request.ModifiedSince.HasValue)
                parameters.Add($"modifiedSince={request.ModifiedSince.Value:yyyy-MM-dd}");
            
            if (request.Limit.HasValue)
                parameters.Add($"limit={request.Limit.Value}");
            
            if (request.Offset.HasValue)
                parameters.Add($"offset={request.Offset.Value}");
            
            if (!string.IsNullOrEmpty(request.OrderBy))
                parameters.Add($"orderBy={request.OrderBy}");
        }

        return parameters.Count > 0 ? "?" + string.Join("&", parameters) : "";
    }

    private string BuildComicQueryParameters(MarvelComicSearchRequest? request = null)
    {
        var parameters = new List<string>();

        // Parámetros de autenticación requeridos por Marvel API
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var hash = GenerateMarvelHash(timestamp);

        parameters.Add($"ts={timestamp}");
        parameters.Add($"apikey={_publicKey}");
        parameters.Add($"hash={hash}");

        // Parámetros de búsqueda opcionales para cómics
        if (request != null)
        {
            if (!string.IsNullOrEmpty(request.Title))
                parameters.Add($"title={Uri.EscapeDataString(request.Title)}");
            
            if (!string.IsNullOrEmpty(request.TitleStartsWith))
                parameters.Add($"titleStartsWith={Uri.EscapeDataString(request.TitleStartsWith)}");
            
            if (request.ModifiedSince.HasValue)
                parameters.Add($"modifiedSince={request.ModifiedSince.Value:yyyy-MM-dd}");
            
            if (request.IssueNumber.HasValue)
                parameters.Add($"issueNumber={request.IssueNumber.Value}");
            
            if (!string.IsNullOrEmpty(request.Isbn))
                parameters.Add($"isbn={Uri.EscapeDataString(request.Isbn)}");
            
            if (!string.IsNullOrEmpty(request.Upc))
                parameters.Add($"upc={Uri.EscapeDataString(request.Upc)}");
            
            if (!string.IsNullOrEmpty(request.DiamondCode))
                parameters.Add($"diamondCode={Uri.EscapeDataString(request.DiamondCode)}");
            
            if (!string.IsNullOrEmpty(request.DigitalId))
                parameters.Add($"digitalId={Uri.EscapeDataString(request.DigitalId)}");
            
            if (!string.IsNullOrEmpty(request.Format))
                parameters.Add($"format={Uri.EscapeDataString(request.Format)}");
            
            if (!string.IsNullOrEmpty(request.FormatType))
                parameters.Add($"formatType={Uri.EscapeDataString(request.FormatType)}");
            
            if (request.NoVariants.HasValue)
                parameters.Add($"noVariants={request.NoVariants.Value.ToString().ToLower()}");
            
            if (!string.IsNullOrEmpty(request.DateDescriptor))
                parameters.Add($"dateDescriptor={Uri.EscapeDataString(request.DateDescriptor)}");
            
            if (request.DateRange.HasValue)
                parameters.Add($"dateRange={request.DateRange.Value:yyyy-MM-dd}");
            
            if (request.StartYear.HasValue)
                parameters.Add($"startYear={request.StartYear.Value}");
            
            if (request.EndYear.HasValue)
                parameters.Add($"endYear={request.EndYear.Value}");
            
            if (request.Limit.HasValue)
                parameters.Add($"limit={request.Limit.Value}");
            
            if (request.Offset.HasValue)
                parameters.Add($"offset={request.Offset.Value}");
            
            if (!string.IsNullOrEmpty(request.OrderBy))
                parameters.Add($"orderBy={request.OrderBy}");
        }

        return parameters.Count > 0 ? "?" + string.Join("&", parameters) : "";
    }

    private string GenerateMarvelHash(string timestamp)
    {
        // Marvel API requiere: md5(ts + privateKey + publicKey)
        var input = timestamp + _privateKey + _publicKey;
        
        using var md5 = MD5.Create();
        var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}
