namespace WebKaraoke.API.Helpers
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public object? Data { get; set; }
        public string? Message { get; set; }

        public static ApiResponse Ok(object? data = null, string? message = null)
        {
            return new ApiResponse
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static ApiResponse Fail(string message)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message
            };
        }
    }
}
