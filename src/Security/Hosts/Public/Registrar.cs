using System.Reflection;
using InsuranceGoSmoke.Common.Cqrs.Extensions;
using InsuranceGoSmoke.Common.Applications.AppServices.Extensions;
using InsuranceGoSmoke.Common.Applications.Handlers.Extensions;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using InsuranceGoSmoke.Common.Hosts.Extensions;
using InsuranceGoSmoke.Security.Public.Hosts.Api.Controllers;
using InsuranceGoSmoke.Common.Contracts;
using System.Text.Json.Serialization;
using InsuranceGoSmoke.Security.ExternalClients.Keycloak;
using InsuranceGoSmoke.Common.Clients.Keycloak;
using InsuranceGoSmoke.Security.Contracts.Identify.Options;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Queries.GetAuthorizationConfig;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Factories.Strategies;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Factories;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services.Abstract;
using InsuranceGoSmoke.Notification.Clients;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;
using InsuranceGoSmoke.Security.Contracts.Common;
using InsuranceGoSmoke.Common.Contracts.Contracts.DI;
using InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Authorization.Instances.Keycloak;
using Microsoft.AspNetCore.Authentication;

namespace InsuranceGoSmoke.Security.Public.Hosts.Api
{
    /// <summary>
    /// Регистратор.
    /// </summary>
    internal static class Registrar
    {
        /// <summary>
        /// Регистрация сервисов.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="hostBuilder">Builder хоста.</param>
        /// <param name="loggingBuilder">Builder логгирования.</param>
        /// <param name="configuration">Конфигурация.</param>
        public static void RegistrarServices(this IServiceCollection services,
            IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            services.AddControllers()
                    .AddJsonOptions(x =>
                    {
                        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    });

            var currentAssembly = typeof(IdentifyController).Assembly;
            services
                .AddEndpointsApiExplorer()
                .AddHandlers(currentAssembly)
                .AddFeatures(hostBuilder, loggingBuilder, configuration,
                [
                    currentAssembly,
                    typeof(IAppFeature).Assembly,
                    typeof(Common.Hosts.Api.Extensions.HostRegistrar).Assembly,
                    typeof(Common.Hosts.Extensions.HostRegistrar).Assembly
                ])
                .AddCommonServices()
                .AddServices()
                .AddClients(configuration)
                .AddTransient(typeof(Lazy<>), typeof(LazyInstance<>));
        }

        /// <summary>
        /// Добавление обработчиков.
        /// </summary>
        /// <param name="currentAssembly">Сборка.</param>
        /// <param name="services">Коллекция сервисов.</param>
        public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly currentAssembly)
        {
            services
                .AddMediatR(currentAssembly)
                .AddAssemblyHandlers(typeof(GetAuthorizationConfigRequestHandler).Assembly);
            return services;
        }

        /// <summary>
        /// Добавление общих сервисов.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services
                .AddDateTimeProvider();
            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

            return services;
        }

        /// <summary>
        /// Добавление сервисов.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IConfigurationService, ConfigurationService>()
                .AddScoped<IAuthorizationService, AuthorizationService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IIdentifyStrategyFactory, IdentifyStrategyFactory>()
                .AddScoped<SmsIdentifyStrategy>()
                .AddConfigurationOptions<IdentifyOptions>();

            return services;
        }

        /// <summary>
        /// Добавление клиента.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="configuration">Конфигурация.</param>
        public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IKeycloakAuthorizationApiClient, KeycloakAuthorizationApiClient>();
            services.AddScoped<IKeycloakUserApiClient, KeycloakUserApiClient>();
            services.AddKeycloakExternalApiClient(configuration);
            services.AddNotificationServiceClient();

            return services;
        }
    }
}
