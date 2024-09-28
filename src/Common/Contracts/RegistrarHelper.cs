using InsuranceGoSmoke.Common.Contracts.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Contracts
{
    /// <summary>
    /// Helper для регистрации.
    /// </summary>
    public static class RegistrarHelper
    {
        /// <summary>
        /// Добавление настроек конфигурации.
        /// </summary>
        /// <typeparam name="TOptions">Настройки.</typeparam>
        /// <param name="services">Колекция сервисов.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IServiceCollection AddConfigurationOptions<TOptions>(this IServiceCollection services) where TOptions : class
        {
            services
               .AddOptions<TOptions>()
               .Configure<IConfiguration>(Configure);

            return services;
        }

        /// <summary>
        /// Настраивает конфигурацию
        /// </summary>
        /// <typeparam name="TOptions">Тип настроек.</typeparam>
        /// <param name="options">Настройки.</param>
        /// <param name="configuration">Конфигурации.</param>
        /// <exception cref="InvalidOperationException">Исключение, когда секция не найдена.</exception>
        public static void Configure<TOptions>(TOptions options, IConfiguration configuration) where TOptions : class
        {
            var attribute = typeof(TOptions).GetCustomAttribute<ConfigurationOptionsAttribute>(inherit: false);
            var sectionName = attribute?.SectionName
                                ?? throw new InvalidOperationException($"Настройки '{typeof(TOptions)}' не помечены атрибутом '{typeof(ConfigurationOptionsAttribute)}'");
            var section = configuration.GetSection(sectionName);
            if (!section.Exists())
            {
                throw new InvalidOperationException(
                    $"Не удалось получить настройки типа '{typeof(TOptions)}': " +
                    $"секция '{sectionName}' не найдена.");
            }
            section.Bind(options);
        }
    }
}
