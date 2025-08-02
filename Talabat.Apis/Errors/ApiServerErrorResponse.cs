namespace Talabat.Apis.Errors
{
    public class ApiServerErrorResponse : ApiResponse
    {
        public ApiServerErrorResponse(int statusCode, string? message = null, string? details = null) : base(statusCode, message)
        {
            Details = details;
        }

        public string? Details { get; set; }
    }
}
