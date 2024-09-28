using MediatR;
using InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DiagnosticProvider;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Session;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Diagnostic;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Transaction;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.DataValidator;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;

namespace InsuranceGoSmoke.Common.Cqrs.Extensions
{
    /// <summary>
    /// Регистрация медиатра и важнейших поведений.
    /// </summary>
    public static class MediatrRegistrar
    {
        /// <summary>
        /// Регистрирует медиатр.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="assembly">Информация о сборке.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddMediatR(this IServiceCollection services, Assembly assembly)
        {
            services
                .AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(assembly);
                })
                .AddScoped<IEventMessageProvider, EventMessageProvider>()
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationRequestBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(AccessValidatorBehaviour<,>))
                .AddScoped<IDiagnosticProvider, DiagnosticProvider>()
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(DiagnosticBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(EventSendingBehavior<,>));

            // Добавляем pipeline транзакций, только если зарегистрирована фабрика доступа к данным.
            if (services.Any(x => x.ServiceType == typeof(IDataSessionFactory)))
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionalBehavior<,>));
            }

            return services;
        }
    }
}
