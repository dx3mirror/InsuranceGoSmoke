using InsuranceGoSmoke.Common.Contracts.Exceptions.Feature;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Swagger;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Swagger
{
    /// <summary>
    /// функциональность swagger.
    /// </summary>
    internal class SwaggerFeature : AppFeature
    {
        public static readonly string JwtTokenScheme = "Bearer";

        public override void AddFeature(IServiceCollection services, IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        {
            base.AddFeature(services, hostBuilder, loggingBuilder);

            AddSwagger(services);
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            base.UseFeature(application, environment);

            application.UseSwagger();
            application.UseSwaggerUI();
        }

        /// <summary>
        /// Регистрация swagger.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <returns>Коллекция сервисов.</returns>
        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(Configure);
        }

        internal void Configure(SwaggerGenOptions options)
        {
            var swaggerOptions = OptionSection?.Get<SwaggerFeatureOptions>()
                            ?? throw new FeatureConfigurationException("В конфигурации не удалось найти настройки Swagger.");

            options.OperationFilter<AddTrustedNetworkHeaderFilter>();
            options.SchemaFilter<AddSwaggerRequiredSchemaFilter>();
            options.DocumentFilter<SwaggerBasePathFilter>(swaggerOptions.BasePath);
            SwaggerConfigure.MapTypes(options);

            options.AddSecurityDefinition(JwtTokenScheme, new OpenApiSecurityScheme
            {
                Description = $"Авторизация через токен. {Environment.NewLine} Введите 'Bearer' [space] и затем токен. {Environment.NewLine} Пример: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtTokenScheme,
                BearerFormat = "Bearer 12345abcdef"
            });

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtTokenScheme
                            },
                            Scheme = "oauth2",
                            Name = JwtTokenScheme,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

            RegisterXmlComments(options);
        }

        private static void RegisterXmlComments(SwaggerGenOptions options)
        {
            var assembly = Assembly.GetExecutingAssembly();
            IncludeXmlComments(options, assembly);

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                IncludeXmlComments(options, entryAssembly);
            }
        }

        private static void IncludeXmlComments(SwaggerGenOptions options, Assembly assembly)
        {
            var xmlFile = $"{assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (Path.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        }
    }
}
