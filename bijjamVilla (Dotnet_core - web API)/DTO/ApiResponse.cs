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
    }
}
