using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurators
{
    /// <summary>
    /// Базовый класс конфигуратора БД.
    /// </summary>
    public abstract class BaseDbContextConfigurator<TDbContext>(
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
        : IDbContextOptionsConfigurator<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Строка подключения.
        /// </summary>
        protected abstract string ConnectionStringName { get; }

        /// <inheritdoc/>
        public void Configure(DbContextOptionsBuilder<TDbContext> options)
        {
            var connectionString = configuration.GetConnectionString(ConnectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Не удалось найти строку подключения '{ConnectionStringName}'");
            }

            options
                .UseLoggerFactory(loggerFactory)
                .UseNpgsql(connectionString, o =>
                {
                    o.CommandTimeout(commandTimeout: 60);
                });
        }
    }
}
