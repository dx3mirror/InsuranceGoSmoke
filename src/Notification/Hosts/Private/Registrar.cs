using System.Reflection;
using InsuranceGoSmoke.Common.Cqrs.Extensions;
using InsuranceGoSmoke.Common.Applications.AppServices.Extensions;
using InsuranceGoSmoke.Common.Applications.Handlers.Extensions;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using InsuranceGoSmoke.Common.Hosts.Extensions;
using InsuranceGoSmoke.Notification.Public.Hosts.Api.Controllers;
using InsuranceGoSmoke.Notification.Applications.Handlers.Contexts.Notifications.Commands.SendEmail;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Notifications.Services.Abstract;
using InsuranceGoSmoke.Notifications.Applications.AppServices.Contexts.Notifications.Services;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Services.Abstract;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Services;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Templates.Services.Abstract;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Templates.Services;
using InsuranceGoSmoke.Common.Contracts;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Models.Options;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Factories;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Factories.Strategies;
using InsuranceGoSmoke.Common.Contracts.Contracts.DI;
using System.Text.Json.Serialization;
using InsuranceGoSmoke.Static.Clients.Options;
using InsuranceGoSmoke.Static.Clients;
using Microsoft.Exchange.WebServices.Data;
using Kortros.Notification.Applications.AppServices.Contexts.Emails.Factories.Strategies.Traciers;

namespace InsuranceGoSmoke.Notification.Private.Hosts.Api
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

            var currentAssembly = typeof(NotificationController).Assembly;
            services
                .AddEndpointsApiExplorer()
                .AddHandlers(currentAssembly)
                .AddFeatures(hostBuilder, loggingBuilder, configuration,
                [
                    currentAssembly,
                    typeof(IAppFeature).Assembly,
                    typeof(Common.Hosts.Api.Extensions.HostRegistrar).Assembly,
                    typeof(HostRegistrar).Assembly
                ])
                .AddCommonServices()
                .AddServices()
                .AddClients()
                .AddStrategies()
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
                .AddAssemblyHandlers(typeof(SendEmailRequestValidator).Assembly);
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
            return services;
        }

        /// <summary>
        /// Добавление сервисов.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<ITraceListener, TraceListener>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<IEmailSendingService, EmailSendingService>()
                .AddScoped<ITemplateService, TemplateService>();
            return services;
        }

        /// <summary>
        /// Добавление стратегий.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        public static IServiceCollection AddStrategies(this IServiceCollection services)
        {
            services
                .AddScoped<IEmailSenderStrategiesFactory, EmailSenderStrategiesFactory>()
                .AddScoped<ExchangeEmailSenderStrategy>()
                .AddScoped<SmtpEmailSenderStrategy>();
            return services;
        }

        /// <summary>
        /// Добавление клиентов.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        public static IServiceCollection AddClients(this IServiceCollection services)
        {
            services
                .AddConfigurationOptions<StaticTemplateOptions>()
                .AddHttpClient<IStaticTemplateClient, StaticTemplateClient>();
            services
                .AddConfigurationOptions<EmailSendingOptions>();
            return services;
        }
    }
}
