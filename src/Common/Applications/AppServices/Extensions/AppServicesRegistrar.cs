using InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DateTimeProvider;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceGoSmoke.Common.Applications.AppServices.Extensions
{
    /// <summary>
    /// Регистрация сервисов.
    /// </summary>
    public static class AppServicesRegistrar
    {
        /// <summary>
        /// Регистрация провайдера для работы с датой и временем.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
        {
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}
