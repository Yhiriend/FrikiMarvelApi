namespace FrikiMarvelApi.Domain.DTOs;

/// <summary>
/// Respuesta principal de la API de Marvel
/// </summary>
/// <typeparam name="T">Tipo de datos que contiene la respuesta</typeparam>
public class MarvelApiResponse<T>
{
    public int Code { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Copyright { get; set; } = string.Empty;
    public string AttributionText { get; set; } = string.Empty;
    public string AttributionHtml { get; set; } = string.Empty;
    public string Etag { get; set; } = string.Empty;
    public MarvelData<T> Data { get; set; } = new();
}

/// <summary>
/// Contenedor de datos de la API de Marvel
/// </summary>
/// <typeparam name="T">Tipo de datos</typeparam>
public class MarvelData<T>
{
    public int Offset { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
    public int Count { get; set; }
    public List<T> Results { get; set; } = new();
}

/// <summary>
/// Personaje de Marvel
/// </summary>
public class MarvelCharacter
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Modified { get; set; } = string.Empty;
    public string ResourceUri { get; set; } = string.Empty;
    public List<MarvelUrl> Urls { get; set; } = new();
    public MarvelImage? Thumbnail { get; set; }
    public MarvelComicList Comics { get; set; } = new();
    public MarvelStoryList Stories { get; set; } = new();
    public MarvelEventList Events { get; set; } = new();
    public MarvelSeriesList Series { get; set; } = new();
}

/// <summary>
/// Imagen de Marvel
/// </summary>
public class MarvelImage
{
    public string Path { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    
    public string GetFullUrl() => $"{Path}.{Extension}";
}

/// <summary>
/// URL de Marvel
/// </summary>
public class MarvelUrl
{
    public string Type { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

/// <summary>
/// Lista de cómics de Marvel
/// </summary>
public class MarvelComicList
{
    public int Available { get; set; }
    public int Returned { get; set; }
    public string CollectionUri { get; set; } = string.Empty;
    public List<MarvelComicSummary> Items { get; set; } = new();
}

/// <summary>
/// Resumen de cómic de Marvel
/// </summary>
public class MarvelComicSummary
{
    public string ResourceUri { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Lista de historias de Marvel
/// </summary>
public class MarvelStoryList
{
    public int Available { get; set; }
    public int Returned { get; set; }
    public string CollectionUri { get; set; } = string.Empty;
    public List<MarvelStorySummary> Items { get; set; } = new();
}

/// <summary>
/// Resumen de historia de Marvel
/// </summary>
public class MarvelStorySummary
{
    public string ResourceUri { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

/// <summary>
/// Lista de eventos de Marvel
/// </summary>
public class MarvelEventList
{
    public int Available { get; set; }
    public int Returned { get; set; }
    public string CollectionUri { get; set; } = string.Empty;
    public List<MarvelEventSummary> Items { get; set; } = new();
}

/// <summary>
/// Resumen de evento de Marvel
/// </summary>
public class MarvelEventSummary
{
    public string ResourceUri { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Lista de series de Marvel
/// </summary>
public class MarvelSeriesList
{
    public int Available { get; set; }
    public int Returned { get; set; }
    public string CollectionUri { get; set; } = string.Empty;
    public List<MarvelSeriesSummary> Items { get; set; } = new();
}

/// <summary>
/// Resumen de serie de Marvel
/// </summary>
public class MarvelSeriesSummary
{
    public string ResourceUri { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Cómic de Marvel
/// </summary>
public class MarvelComic
{
    public int Id { get; set; }
    public int DigitalId { get; set; }
    public string Title { get; set; } = string.Empty;
    public double IssueNumber { get; set; }
    public string VariantDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Modified { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public string Upc { get; set; } = string.Empty;
    public string DiamondCode { get; set; } = string.Empty;
    public string Ean { get; set; } = string.Empty;
    public string Issn { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public List<MarvelTextObject> TextObjects { get; set; } = new();
    public string ResourceUri { get; set; } = string.Empty;
    public List<MarvelUrl> Urls { get; set; } = new();
    public MarvelImage? Thumbnail { get; set; }
    public List<MarvelImage> Images { get; set; } = new();
    public MarvelCreatorList Creators { get; set; } = new();
    public MarvelCharacterList Characters { get; set; } = new();
    public MarvelStoryList Stories { get; set; } = new();
    public MarvelEventList Events { get; set; } = new();
    public List<MarvelComicDate> Dates { get; set; } = new();
    public List<MarvelComicPrice> Prices { get; set; } = new();
}

/// <summary>
/// Objeto de texto de Marvel
/// </summary>
public class MarvelTextObject
{
    public string Type { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

/// <summary>
/// Lista de creadores de Marvel
/// </summary>
public class MarvelCreatorList
{
    public int Available { get; set; }
    public int Returned { get; set; }
    public string CollectionUri { get; set; } = string.Empty;
    public List<MarvelCreatorSummary> Items { get; set; } = new();
}

/// <summary>
/// Resumen de creador de Marvel
/// </summary>
public class MarvelCreatorSummary
{
    public string ResourceUri { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

/// <summary>
/// Lista de personajes de Marvel
/// </summary>
public class MarvelCharacterList
{
    public int Available { get; set; }
    public int Returned { get; set; }
    public string CollectionUri { get; set; } = string.Empty;
    public List<MarvelCharacterSummary> Items { get; set; } = new();
}

/// <summary>
/// Resumen de personaje de Marvel
/// </summary>
public class MarvelCharacterSummary
{
    public string ResourceUri { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

/// <summary>
/// Fecha de cómic de Marvel
/// </summary>
public class MarvelComicDate
{
    public string Type { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}

/// <summary>
/// Precio de cómic de Marvel
/// </summary>
public class MarvelComicPrice
{
    public string Type { get; set; } = string.Empty;
    public double Price { get; set; }
}

/// <summary>
/// DTO para búsqueda de personajes
/// </summary>
public class MarvelCharacterSearchRequest
{
    public string? Name { get; set; }
    public string? NameStartsWith { get; set; }
    public DateTime? ModifiedSince { get; set; }
    public int? Limit { get; set; } = 20;
    public int? Offset { get; set; } = 0;
    public string? OrderBy { get; set; } = "name";
}

/// <summary>
/// DTO para búsqueda de cómics
/// </summary>
public class MarvelComicSearchRequest
{
    public string? Title { get; set; }
    public string? TitleStartsWith { get; set; }
    public DateTime? ModifiedSince { get; set; }
    public int? IssueNumber { get; set; }
    public string? Isbn { get; set; }
    public string? Upc { get; set; }
    public string? DiamondCode { get; set; }
    public string? DigitalId { get; set; }
    public string? Format { get; set; }
    public string? FormatType { get; set; }
    public bool? NoVariants { get; set; }
    public string? DateDescriptor { get; set; }
    public DateTime? DateRange { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public int? Limit { get; set; } = 20;
    public int? Offset { get; set; } = 0;
    public string? OrderBy { get; set; } = "title";
}
