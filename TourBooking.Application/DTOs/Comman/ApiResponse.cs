namespace TourBooking.Application.DTOs.Comman
{
     public class ApiResponse<T> : BaseResponse
    {
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T? data, string message, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Data = data,
                Message = message,
                StatusCode = statusCode,
                IsSuccess = true,
                HasExceptionError = false,
                ValidationErrors = null
            };
        }

        public static ApiResponse<T> FailResponse(string message, int statusCode = 400, List<string>? validationErrors = null)
        {
            return new ApiResponse<T>
            {
                Data = default,
                Message = message,
                StatusCode = statusCode,
                IsSuccess = false,
                HasExceptionError = false,
                ValidationErrors = validationErrors
            };
        }

        public static ApiResponse<T> ExceptionResponse(string message)
        {
            return new ApiResponse<T>
            {
                Data = default,
                Message = message,
                StatusCode = 500,
                IsSuccess = false,
                HasExceptionError = true,
                ValidationErrors = null
            };
        }
    }
}