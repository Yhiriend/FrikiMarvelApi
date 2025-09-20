namespace FrikiMarvelApi.Domain.Models;

/// <summary>
/// Modelo de respuesta estandarizada para todos los endpoints de la API
/// </summary>
/// <typeparam name="T">Tipo de datos que contiene la respuesta</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public string? Error { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operación exitosa")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> ErrorResponse(string error, string message = "Error en la operación")
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Error = error
        };
    }
}

/// <summary>
/// Modelo de respuesta estandarizada sin datos específicos
/// </summary>
public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse SuccessResponse(string message = "Operación exitosa")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message
        };
    }

    public static new ApiResponse ErrorResponse(string error, string message = "Error en la operación")
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Error = error
        };
    }
}
