using InsuranceGoSmoke.Common.Contracts.Exceptions.Feature;
using InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.ExceptionHandler;
using InsuranceGoSmoke.Common.Hosts.Extensions;
using InsuranceGoSmoke.Common.Hosts.Models.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Text.Json;

namespace InsuranceGoSmoke.Common.Hosts.Api.Filters.RateLimit
{
    /// <summary>
    /// Атрибут над методами для ограничения количества попыток.
    /// </summary>
    public class RateLimitAttribute : ActionFilterAttribute
    {
        private readonly int _maxAttempts;
        private readonly int _delaySeconds;
        private readonly string? _uniqueParameter;

        /// <summary>
        /// Создаёт экземпляр <see cref="RateLimitAttribute"/>
        /// </summary>
        /// <param name="maxAttempts">Максимальное количество попыток.</param>
        /// <param name="delaySeconds">Период времени в секундах</param>
        public RateLimitAttribute(int maxAttempts, int delaySeconds)
        {
            _maxAttempts = maxAttempts;
            _delaySeconds = delaySeconds;
        }

        /// <summary>
        /// Создаёт экземпляр <see cref="RateLimitAttribute"/>
        /// </summary>
        /// <param name="maxAttempts">Максимальное количество попыток.</param>
        /// <param name="delaySeconds">Период времени в секундах</param>
        /// <param name="uniqueParameter">Уникальный параметр</param>
        public RateLimitAttribute(int maxAttempts, int delaySeconds, string uniqueParameter) 
            : this(maxAttempts, delaySeconds)
        {
            _uniqueParameter = uniqueParameter;
        }

        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = context.HttpContext.RequestServices.GetService<IDistributedCache>() 
                        ?? throw new FeatureConfigurationException("Для ограничения попыток вызова методов, необходимо зарегистрировать Redis.");

            var key = GenerateClientKey(context);
            var attemptData = await cache.GetStringAsync(key);

            int attempts = 0;
            if (!string.IsNullOrEmpty(attemptData))
            {
                attempts = JsonSerializer.Deserialize<int>(attemptData);
            }

            if (attempts >= _maxAttempts)
            {
                context.HttpContext.Response.Headers.Append("Retry-After", new string[] { _delaySeconds.ToString() });
                context.Result = new ContentResult
                {
                    ContentType = "application/json",
                    Content = JsonSerializer.Serialize(
                        new ApiError($"Слишком много попыток. Повторите позже (через {_delaySeconds} сек.)."), 
                            ExceptionHandlingMiddleware.JsonSerializerOptions),
                    StatusCode = (int)HttpStatusCode.TooManyRequests
                };
                return;
            }

            attempts++;
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_delaySeconds)
            };
            await cache.SetStringAsync(key, JsonSerializer.Serialize(attempts), options);

            await next();
        }

        private string GenerateClientKey(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            IPAddress? originalIp = IPHelper.GetOriginalIp(request);
            var clientIp = originalIp?.ToString() ?? "unknown_ip";
            var path = request.Path.ToString().ToLowerInvariant();

            if (string.IsNullOrEmpty(_uniqueParameter))
            {
                return $"{clientIp}:{path}";
            }

            var value = (string?)context.ModelState[_uniqueParameter]?.RawValue;
            if (string.IsNullOrEmpty(value))
            {
                return $"{clientIp}:{path}";
            }

            return $"{clientIp}:{path}:{value}";
        }
    }
}
