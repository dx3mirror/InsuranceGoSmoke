using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using InsuranceGoSmoke.Common.Clients.Keycloak;
using Microsoft.IdentityModel.Tokens;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using System.Security.Claims;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers.Access;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Authorization.Instances.Keycloak
{
    /// <summary>
    /// Расширение для авторизации через Keycloak.
    /// </summary>
    internal static class KeycloakAuthorizationFeatureExtension
    {
        /// <summary>
        /// Регистрирует компоненты для аутентификации через Keycloak.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="optionSection">Конфигурация.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddKeycloakAuthenticate(this IServiceCollection services, IConfigurationSection optionSection)
        {
            var keycloakAuthorizationOptions = optionSection.GetSection(KeycloakAuthorizationOptions.Scheme).Get<KeycloakAuthorizationOptions>();
            services.AddHttpClient();
            services.AddOptions<KeycloakAuthorizationOptions>()
                    .Configure(o =>
                    {
                        optionSection.Bind(KeycloakAuthorizationOptions.Scheme, o);
                    });

            // Строка нужна, чтобы поле sub в claims не очищалось
            JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear(); 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        GetJwtBearerSettings(options, keycloakAuthorizationOptions);
                    });

            services.AddHttpContextAccessor();
            services.AddScoped<IAuthorizationData>(provider =>
            {
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var context = httpContextAccessor.HttpContext ?? throw new AuthorizationException("Контекст доступа пуст.");
                var sub = context.User.FindFirst(JwtRegisteredClaimNames.Sub);
                if (!Guid.TryParse(sub?.Value, out var userId))
                {
                    return new AuthorizationData();
                }
                var roles = GetRoles(context.User);

                return new AuthorizationData(userId)
                {
                    Roles = roles
                };
            });

            return services;
        }

        /// <summary>
        /// Возвращает настройки для авторизации по токену.
        /// </summary>
        /// <param name="options">Настройки токена.</param>
        /// <param name="keycloakAuthorizationOptions">настройки keycloak</param>
        internal static void GetJwtBearerSettings(JwtBearerOptions options, KeycloakAuthorizationOptions? keycloakAuthorizationOptions)
        {
            if (keycloakAuthorizationOptions == null)
            {
                return;
            }

            options.Authority = keycloakAuthorizationOptions.Authority;
            options.Audience = keycloakAuthorizationOptions.ClientId;
            options.RequireHttpsMetadata = keycloakAuthorizationOptions.RequireHttpsMetadata;
            options.MetadataAddress = keycloakAuthorizationOptions.MetadataAddress;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,

                ValidateAudience = true,
                ValidAudience = keycloakAuthorizationOptions.ClientId
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    context.NoResult();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "text/plain";
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Append("Token-Expired", bool.TrueString);
                    }

                    var message = "При авторизация произошла ошибка: " + context.Exception.Message;
                    context.Response.WriteAsync(message).Wait();
                    context.Response.CompleteAsync().Wait();
                    return Task.CompletedTask;
                }
            };
        }

        private static IReadOnlyCollection<RoleTypes> GetRoles(ClaimsPrincipal principal)
        {
            var roleClaim = principal.FindFirst(ClaimTypes.Role);
            if (roleClaim is null)
            {
                return [];
            }

            var role = RoleTypeHelper.GetRole(roleClaim.Value);
            return [role];
        }
    }
}
