using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Expactions;


namespace TourBooking.API.Exceptions
{
    
        public class ExceptionHandlingMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<ExceptionHandlingMiddleware> _logger;

            public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    // Hatanın detaylarını loglamak çok önemlidir!
                    _logger.LogError(ex, "An unhandled exception has occurred. Message: {Message}", ex.Message);
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
                            "Doğrulama hatası oluştu.",
                            context.Response.StatusCode,
                            validationEx.Errors
                        );
                        break;

                    case BusinessRuleValidationException businessRuleEx:
                        context.Response.StatusCode = (int)HttpStatusCode.Conflict; // 409
                        errorResponse = ApiResponse<object>.FailResponse(
                            businessRuleEx.Message,
                            context.Response.StatusCode
                        );
                        break;

                    case NotFoundException notFoundEx:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404
                        errorResponse = ApiResponse<object>.FailResponse(
                            notFoundEx.Message,
                            context.Response.StatusCode
                        );
                        break;

                    case AuthenticationFailedException authEx:
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized; // 401
                        errorResponse = ApiResponse<object>.FailResponse(
                            authEx.Message,
                            context.Response.StatusCode
                        );
                        break;

                    default: // Tanımlamadığımız diğer tüm hatalar (sistem hataları vb.)
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                        errorResponse = ApiResponse<object>.ExceptionResponse(
                            "Beklenmedik bir sunucu hatası oluştu. Lütfen daha sonra tekrar deneyin."
                        );
                        break;
                }

                var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Mobil taraf için camelCase daha standarttır.
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull

                });
                await context.Response.WriteAsync(result);
            }
        }

    }
