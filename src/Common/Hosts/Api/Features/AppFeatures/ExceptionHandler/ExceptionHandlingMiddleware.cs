using FluentValidation;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Authorization.Instances.TrustedNetwork;
using InsuranceGoSmoke.Common.Hosts.Models.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.ExceptionHandler
{
    /// <summary>
    /// Middleware для обработки исключений.
    /// </summary>
    /// <param name="_next">Делегат.</param>
    public sealed class ExceptionHandlingMiddleware(RequestDelegate _next)
    {
        private const string LogTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode}";
        private static readonly IReadOnlyCollection<string> HeaderWhiteList =
            new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                "User-Agent",
                "Content-Length",
                "Content-Type",
                "Referer",
                "X-Forwarded-Host",
                "X-Forwarded-Server",
                "X-Real-IP",
                "X-Forwarded-For",
                "Host"
            };
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All),
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <param name="logger">Логгер.</param>
        /// <param name="environment">Окружение.</param>
        /// <param name="serviceProvider">Провайдер сервисов.</param>
        /// <exception cref="ArgumentNullException">Исключение, вырасываемое когда аргумент не задан.</exception>
        public async Task Invoke(
            HttpContext context
            , ILogger<ExceptionHandlingMiddleware> logger
            , IHostEnvironment environment
            , IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var statusCode = GetStatusCode(context, exception);

                Log(context, logger, exception, statusCode);

                var apiError = CreateApiError(context, exception, environment, serviceProvider);
                var response = JsonSerializer.Serialize(apiError, JsonSerializerOptions);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;
                await context.Response.WriteAsync(response);
            }
        }

        private static void Log(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger, Exception exception, HttpStatusCode statusCode)
        {
            var headers = context.Request.Headers
                            .Where(h => HeaderWhiteList.Contains(h.Key))
                            .ToDictionary(v => v.Key, v => v.Value.ToString());
            var path = context.Features.Get<IHttpRequestFeature>()?.RawTarget ?? context.Request.Path.ToString();

            using (LogContext.PushProperty("Request.TraceIdentifier", context.TraceIdentifier))
            using (LogContext.PushProperty("Request.UserIdentityName", context.User?.Identity?.Name ?? string.Empty))
            using (LogContext.PushProperty("Request.ConnectionIpAddress", context.Connection?.RemoteIpAddress?.ToString() ?? string.Empty))
            using (LogContext.PushProperty("Request.Headers", headers, destructureObjects: true))
            using (LogContext.PushProperty("Request.DisplayUrl", context.Request.GetDisplayUrl()))
            {
                logger.LogError(exception, LogTemplate, context.Request.Method, path, (int)statusCode);
            }
        }

        internal static ApiError CreateApiError(
            HttpContext context, Exception exception, IHostEnvironment environment, IServiceProvider serviceProvider)
        {
            var traceId = GetTraceIdentifier(context);

            if (!environment.IsDevelopment())
            {
                var trustedNetworkValidator = serviceProvider.GetService<ITrustedNetworkValidator>();
                if (trustedNetworkValidator != null)
                {
                    var trustedNetworkValidationResult = trustedNetworkValidator.Validate(context.Request);
                    if (!trustedNetworkValidationResult)
                    {
                        return new ApiError(traceId,
                            $"Произошло исключение '{exception.GetType().FullName}': {exception.Message}")
                        { Description = exception.ToString() };
                    }
                }
            }

            if (environment.IsDevelopment())
            {
                return new ApiError(traceId,
                    $"Произошло исключение '{exception.GetType().FullName}': {exception.Message}")
                { Description = exception.ToString() };
            }
            if (TryGetException<UnauthorizedAccessException>(exception, out _))
            {
                return new ApiError(traceId, "Отсутствуют права на выполнение действия");
            }
            if (TryGetException<OperationCanceledException>(exception, out _))
            {
                return new ApiError(traceId, "Операция была отменена");
            }
            if (TryGetException<ValidationException>(exception, out var ve) && ve is not null)
            {
                return new ApiError(traceId, "Переданы невалидные данные") { Description = ve.Message };
            }
            if (TryGetException<ReadableException>(exception, out var re) && re is not null)
            {
                return new ApiError(traceId, re.Message) { Description = re.Description };
            }
            if (TryGetException<AccessDeniedException>(exception, out var ade) && ade is not null)
            {
                return new ApiError(traceId, ade.Message);
            }

            return new ApiError(traceId, "Произошла непредвиденная ошибка")
            {
                Description = "Произошла ошибка при выполнении запроса. Пожалуйста, попробуйте позже."
                        + "Если ошибка повторяется, обратитесь к администратору системы или службе технической поддержки для получения дополнительной помощи"
            };
        }

        private static HttpStatusCode GetStatusCode(HttpContext context, Exception exception)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("token/refresh"))
            {
                return HttpStatusCode.Unauthorized;
            }

            return exception switch
            {
                UnauthorizedAccessException _ => HttpStatusCode.Forbidden,
                ValidationException _ => HttpStatusCode.BadRequest,
                OperationCanceledException _ => HttpStatusCode.RequestTimeout,
                NotFoundException _ => HttpStatusCode.NotFound,
                AccessDeniedException _ => HttpStatusCode.Forbidden,
                _ => HttpStatusCode.InternalServerError,
            };
        }

        private static string GetTraceIdentifier(HttpContext context)
        {
            var activity = Activity.Current;
            if (activity == null)
            {
                return context.TraceIdentifier;
            }

            return GetTraceId(activity);
        }

        private static string GetTraceId(Activity activity)
        {
            ArgumentNullException.ThrowIfNull(activity);

            var traceId = activity.IdFormat switch
            {
                ActivityIdFormat.Hierarchical => activity.RootId,
                ActivityIdFormat.W3C => activity.TraceId.ToHexString(),
                ActivityIdFormat.Unknown => null,
                _ => null,
            };

            return traceId ?? string.Empty;
        }

        private static bool TryGetException<TException>(Exception source, out TException? exception)
            where TException : Exception
        {
            exception = null;
            var current = source;
            while (current != null)
            {
                exception = current as TException;
                if (exception != null)
                {
                    return true;
                }

                current = current.InnerException;
            }

            return false;
        }
    }
}
