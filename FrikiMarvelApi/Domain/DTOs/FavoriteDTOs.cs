using System.ComponentModel.DataAnnotations;

namespace FrikiMarvelApi.Domain.DTOs;

/// <summary>
/// DTO para agregar un cómic a favoritos
/// </summary>
public class AddComicFavoriteRequest
{
    [Required]
    public int ComicId { get; set; }
    
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
    
    [Required]
    public string Format { get; set; } = string.Empty;
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string OnSaleDate { get; set; } = string.Empty;
    
    [Required]
    public string Author { get; set; } = string.Empty;
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public string Characters { get; set; } = string.Empty; // Lista de personajes separados por coma
}

/// <summary>
/// DTO para representar un cómic favorito
/// </summary>
public class ComicFavoriteDto
{
    public int ComicId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string OnSaleDate { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Characters { get; set; } = string.Empty;
    public DateTime AddedDate { get; set; }
}

/// <summary>
/// DTO para respuesta de favoritos
/// </summary>
public class ComicFavoritesResponse
{
    public List<ComicFavoriteDto> Favorites { get; set; } = new();
    public int TotalCount { get; set; }
}
