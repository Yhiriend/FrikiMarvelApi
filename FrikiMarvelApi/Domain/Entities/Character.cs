namespace FrikiMarvelApi.Domain.Entities;

public class Character : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string MarvelId { get; set; } = string.Empty;
    public DateTime LastSync { get; set; } = DateTime.UtcNow;
}
