using FluentValidation;
using MediatR;
using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Applications.Handlers.Extensions
{
    /// <summary>
    /// Регистрация обработчиков.
    /// </summary>
    public static class HandlerRegistrar
    {
        /// <summary>
        /// Регистрация валидаторов и обработчиков.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="assembly">Сборка.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddAssemblyHandlers(this IServiceCollection services, Assembly? assembly)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(assembly);

            services
                .AddAccessValidatorHandlers(assembly)
                .AddValidators(assembly)
                .AddRequestHandlers(assembly)
                .AddEventHandlers(assembly)
                .AddDiagnosticHandlers(assembly);

            return services;
        }

        /// <summary>
        /// Регистрирует валидаторы.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="assembly">Данные сборки.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddValidators(this IServiceCollection services, Assembly? assembly)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(assembly);

            services.AddValidatorsFromAssembly(assembly, ServiceLifetime.Transient);

            return services;
        }

        /// <summary>
        /// Регистрирует обработчики запросов.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="assembly">Данные сборки.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddRequestHandlers(this IServiceCollection services, Assembly? assembly)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(assembly);

            Type[] interfaces = [typeof(IQueryHandler<,>), typeof(ICommandHandler<,>), typeof(ICommandHandler<>)];
            var handlerTypes = assembly.DefinedTypes
                                .Where(t => !t.IsOpenGeneric() && t.IsConcrete())
                                .Where(t => Array.Exists(t.GetInterfaces(), i => i.IsGenericType && interfaces.Contains(i.GetGenericTypeDefinition())))
                                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                var handlerInterfaces = handlerType.GetInterfaces()
                                                   .Where(i => i.IsGenericType 
                                                            && (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                                                             || i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)));
                foreach (var handlerInterface in handlerInterfaces)
                {
                    services.TryAddScoped(handlerInterface, handlerType);
                }
            }

            return services;
        }

        /// <summary>
        /// Регистрирует обработчики диагностики.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="assembly">Данные сборки.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddDiagnosticHandlers(this IServiceCollection services, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(assembly);

            Type[] interfaces = [typeof(IDiagnosticHandler<>), typeof(IDiagnosticHandler<,>)];
            var handlerTypes = assembly.DefinedTypes
                                .Where(t => !t.IsAbstract && !t.IsInterface)
                                .Where(t => Array.Exists(t.GetInterfaces(),
                                                i => i.IsGenericType && interfaces.Contains(i.GetGenericTypeDefinition())))
                                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                var handlerInterfaces = handlerType.GetInterfaces().Where(i => i.IsGenericType);
                foreach (var handlerInterface in handlerInterfaces)
                {
                    services.TryAddScoped(handlerInterface, handlerType);
                }
            }

            return services;
        }

        /// <summary>
        /// Регистрирует обработчики проверки доступов.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="assembly">Данные сборки.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddAccessValidatorHandlers(this IServiceCollection services, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(assembly);

            Type[] interfaces = [typeof(IAccessValidatorHandler<>), typeof(IAccessValidatorHandler<,>)];
            var handlerTypes = assembly.DefinedTypes
                                .Where(t => !t.IsAbstract && !t.IsInterface)
                                .Where(t => Array.Exists(t.GetInterfaces(),
                                                i => i.IsGenericType && interfaces.Contains(i.GetGenericTypeDefinition())))
                                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                var handlerInterfaces = handlerType.GetInterfaces().Where(i => i.IsGenericType);
                foreach (var handlerInterface in handlerInterfaces)
                {
                    services.TryAddScoped(handlerInterface, handlerType);
                }
            }

            return services;
        }

        /// <summary>
        /// Регистрирует обработчики событий.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="assembly">Данные сборки.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddEventHandlers(this IServiceCollection services, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(assembly);

            Type[] interfaces = [typeof(IEventHandler)];
            var handlerTypes = assembly.DefinedTypes
                                .Where(t => !t.IsAbstract && !t.IsInterface)
                                .Where(t => Array.Exists(t.GetInterfaces(), i => interfaces.Contains(i)))
                                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                var registerTypeHandler = handlerType.BaseType;
                if (registerTypeHandler is not null)
                {
                    services.TryAddScoped(registerTypeHandler, handlerType);
                }
            }

            return services;
        }

        /// <summary>
        /// Возвращает признак того, что тип может быть инстанциирован (не абстрактный и не интерфейс).
        /// </summary>
        private static bool IsConcrete(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }

        /// <summary>
        /// Возвращает признак того, что тип является open-generic типом.
        /// </summary>
        private static bool IsOpenGeneric(this Type type)
        {
            return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
        }
    }
}
