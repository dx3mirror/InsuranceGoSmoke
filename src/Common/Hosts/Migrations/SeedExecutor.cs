using InsuranceGoSmoke.Common.Hosts.Migrations.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace InsuranceGoSmoke.Common.Hosts.Migrations
{
    /// <summary>
    /// Применение Seed'ов.
    /// </summary>
    /// <typeparam name="TContext">Тип контекста базы данных.</typeparam>
    public class SeedExecutor<TContext> : ISeedExecutor
        where TContext : DbContext
    {
        private readonly TContext _dbContext;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="dbContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SeedExecutor(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Применение Seed'ов.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var type = typeof(ISeed<TContext>);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .Where(t => type.IsAssignableFrom(t) && !t.IsInterface);

            foreach (var seedType in types)
            {
                var seed = (ISeed<TContext>?)Activator.CreateInstance(seedType);
                if (seed != null)
                {
                    await seed.FillAsync(_dbContext, cancellationToken);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
