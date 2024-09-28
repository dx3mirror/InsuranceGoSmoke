using InsuranceGoSmoke.Common.Consumers.Options;
using InsuranceGoSmoke.Common.Consumers.Services.Builders;
using InsuranceGoSmoke.Common.Consumers.Services.MessageSender;
using InsuranceGoSmoke.Common.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceGoSmoke.Common.Consumers
{
    /// <summary>
    /// Регистрация компонентов.
    /// </summary>
    public static class ConsumerRegistrar
    {
        /// <summary>
        /// Регистрирует сервисы для отправителя.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <returns>Сервисы.</returns>
        public static IServiceCollection AddProducerServices(this IServiceCollection services)
        {
            services.AddConfigurationOptions<KafkaOptions>();
            services.AddTransient<IKafkaProducerBuilder, KafkaProducerBuilder>();
            services.AddScoped<IMessageSenderService, MessageSenderService>();

            return services;
        }

        /// <summary>
        /// Регистрирует сервисы для получателя.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <returns>Сервисы.</returns>
        public static IServiceCollection AddConsumerServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
