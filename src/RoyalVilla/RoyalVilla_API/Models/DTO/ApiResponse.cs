namespace StayHub.Api.Models.DTO;

public class ApiResponse<TData>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
    public object? Errors { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<TData> Create(bool success, int statusCode, string message, TData? data = default, object? errors = null)
    {
        return new ApiResponse<TData>
        {
            Success = success,
            StatusCode = statusCode,
            Message = message,
            Data = data,
            Errors = errors
        };
    }

    public static ApiResponse<TData> Ok(TData data, string message) => Create(true, StatusCodes.Status200OK, message, data);

    public static ApiResponse<TData> CreatedAt(TData data, string message) => Create(true, StatusCodes.Status200OK, message, data);

    public static ApiResponse<TData> NoContent(string message = "Operation completed successfully") => Create(true, StatusCodes.Status204NoContent, message);

    public static ApiResponse<TData> NotFound(string message = "Resource not found") => Create(false, StatusCodes.Status404NotFound, message);

    public static ApiResponse<TData> BadRequest(string message, object? errors = null) => Create(false, StatusCodes.Status400BadRequest, message, errors: errors);

    public static ApiResponse<TData> Conflict(string message) => Create(false, StatusCodes.Status409Conflict, message);
    public static ApiResponse<TData> Error(int statusCode, string message, object? errors = null) => Create(false, statusCode, message, errors: errors);
}
