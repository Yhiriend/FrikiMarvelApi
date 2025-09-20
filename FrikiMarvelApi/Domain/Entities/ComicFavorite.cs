using System.ComponentModel.DataAnnotations;

namespace FrikiMarvelApi.Domain.Entities;

/// <summary>
/// Entidad para almacenar cómics favoritos de los usuarios
/// </summary>
public class ComicFavorite : BaseEntity
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public string ComicData { get; set; } = string.Empty; // JSON string con los datos del cómic
    
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    
    // Navegación
    public User User { get; set; } = null!;
}
