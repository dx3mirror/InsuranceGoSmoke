using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurations;
using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Repositories;
using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceGoSmoke.Common.Test.Infrastructure
{
    /// <summary>
    /// Базовый класс для тестов с использованием базы данных в памяти.
    /// </summary>
    /// <typeparam name="TContext">Тип контекста базы.</typeparam>
    /// <typeparam name="TConfigurator">Тип конфигрутора контекста базы данных.</typeparam>
    public class BaseMemoryDatabaseTests<TContext, TConfigurator> 
        where TContext : DbContext
        where TConfigurator : class, IDbContextOptionsConfigurator<TContext>
    {
        /// <summary>
        /// Провайдер сервисов.
        /// </summary>
        protected ServiceProvider _serviceProvider;

        /// <summary>
        /// Сервисы.
        /// </summary>
        protected readonly static ServiceCollection _services = new ServiceCollection();

        /// <summary>
        /// Инициализация.
        /// </summary>
        public void Init()
        {
            _services.AddDbContext<TContext>(options =>
                options.UseInMemoryDatabase("TestDb")
                       .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            _serviceProvider = AddDataAccess(_services)
                                .BuildServiceProvider();
        }

        /// <summary>
        /// Регистрирует классы для доступа к базе данных.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <returns>Коллекция сервисов.</returns>
        private static IServiceCollection AddDataAccess(IServiceCollection services)
        {
            services.AddDbContext<TContext>(options =>
                options.UseInMemoryDatabase("TestDb"));

            services
                .AddSingleton<IDbContextOptionsConfigurator<TContext>, TConfigurator>()
                .AddScoped<DbContext>(sp => sp.GetRequiredService<TContext>())
                .AddScoped<IDataSessionFactory, EntityFrameworkDataSessionFactory>()
                .AddScoped(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));

            return services;
        }

        /// <summary>
        /// Очистка.
        /// </summary>
        public void Cleanup()
        {
            var dbContext = _serviceProvider?.GetService<TContext>();

            dbContext?.Database.EnsureDeleted();
        }

    }
}
