using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Localization;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Expactions;
using TourBooking.Shared.Localization;

namespace TourBooking.API.Exceptions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IStringLocalizer<SharedResource> localizer
        )
        {
            _next = next;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var localizedMessage = _localizer["AnUnhandledExceptionOccurred", ex.Message];
                _logger.LogError(ex, "{Message}", localizedMessage);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Hata durumunda T'nin bir önemi olmadığı için ApiResponse<object> kullanıyoruz.
            ApiResponse<object> errorResponse;

            switch (exception)
            {
                case ValidationException validationEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                    errorResponse = ApiResponse<object>.FailResponse(
                        _localizer[validationEx.Message],
                        context.Response.StatusCode,
                        validationEx.Errors
                    );
                    break;

                case BusinessRuleValidationException businessRuleEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict; // 409
                    errorResponse = ApiResponse<object>.FailResponse(
                        _localizer[businessRuleEx.Message],
                        context.Response.StatusCode
                    );
                    break;

                case NotFoundException notFoundEx:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404
                    errorResponse = ApiResponse<object>.FailResponse(
                        _localizer[notFoundEx.Message],
                        context.Response.StatusCode
                    );
                    break;

                case AuthenticationFailedException authEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized; // 401
                    errorResponse = ApiResponse<object>.FailResponse(
                        _localizer[authEx.Message],
                        context.Response.StatusCode
                    );
                    break;

                case NotConfimedException notConfirmed:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden; // 403
                    errorResponse = ApiResponse<object>.FailResponse(
                        _localizer[notConfirmed.Message],
                        context.Response.StatusCode
                    );
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    // Kullanıcıya görünen mesaj (resx üzerinden)
                    var userFriendlyMessage = _localizer["UnexpectedServerError"];
                    errorResponse = ApiResponse<object>.ExceptionResponse(userFriendlyMessage);

                    // Google Cloud Logging'e düşecek gerçek hata
                    _logger.LogError(exception, "Unhandled exception occurred. Message: {Message}", exception.Message);

                    break;


            }

            var result = JsonSerializer.Serialize(
                errorResponse,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Mobil taraf için camelCase daha standarttır.
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }
            );
            await context.Response.WriteAsync(result);
        }
    }
}
