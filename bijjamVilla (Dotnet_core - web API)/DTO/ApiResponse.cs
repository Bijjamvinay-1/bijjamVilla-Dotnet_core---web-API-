namespace bijjamVilla__Dotnet_core___web_API_.DTO
{
    public class ApiResponse<TData>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;      // = string.Empty is used to avoid null reference exception when Message is accessed without being set.
        public TData? Data { get; set; }
        public object? Errors { get; set; } 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;    // Set default value to current UTC time when the response is created.

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

        public static ApiResponse<TData> OK(TData data, string message) =>
           Create(true, 200, message, data);

        public static ApiResponse<TData> CreatedAt(TData data, string message) =>
           Create(true, 201, message, data);

        public static ApiResponse<TData> Nocontent(string message = "Operation completed successfully") =>
            Create(false, 404, message);
        public static ApiResponse<TData> NotFound(string message ="Resource not found") =>
            Create(false, 404, message);

        public static ApiResponse<TData> Badrequest(string message, object? errors = null) =>
            Create(false, 400, message, errors: errors);

        public static ApiResponse<TData> Conflict(string message) =>
            Create(false, 409, message);

        public static ApiResponse<TData> Error(int StatusCode, string message, object? errors = null) =>
            Create(false, StatusCode, message, errors: errors);
    }

}
