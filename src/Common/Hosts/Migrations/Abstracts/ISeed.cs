using Microsoft.EntityFrameworkCore;

namespace InsuranceGoSmoke.Common.Hosts.Migrations.Abstracts
{
    /// <summary>
    /// Интерфейс seed'инга.
    /// </summary>
    /// <typeparam name="TContext">Тип контекста.</typeparam>
    internal interface ISeed<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Заполнение данными.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task FillAsync(TContext context, CancellationToken cancellationToken);
    }
}
