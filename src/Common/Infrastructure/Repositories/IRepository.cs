using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace InsuranceGoSmoke.Common.Infrastructures.DataAccess.Repositories
{
    /// <summary>
    /// Репозиторий для доступа к данным.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Возвращает нематериализованный список сущностей.
        /// </summary>
        /// <returns>Нематериализованный список сущностей.</returns>
        IQueryable<TEntity> AsQueryable();

        /// <summary>
        /// Добавляет сущность в хранилище.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет список сущностей в хранилище.
        /// </summary>
        /// <param name="entities">Сущности.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task AddRangeAsync(TEntity[] entities, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает нематериализованный список отфильтрованных сущностей.
        /// </summary>
        /// <param name="expression">Выражение для фильтрации.</param>
        /// <returns>Нематериализованный список отфильтрованных сущностей.</returns>
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Обновляет данные сущности.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет данные.
        /// </summary>
        /// <param name="entities">Сущности.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken);

        /// <summary>
        /// Выполняет сырой запрос.
        /// </summary>
        /// <param name="sql">Запрос.</param>
        /// <param name="parameters">Параметры.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task ExecuteRawSqlAsync(string sql, IReadOnlyCollection<object> parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Начинает транзакцию.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Экземпляр транзакции.</returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Завершает транзакцию.
        /// </summary>
        /// <param name="transaction">Транзакция.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);

        /// <summary>
        /// Откатывает транзакцию.
        /// </summary>
        /// <param name="transaction">Транзакция.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
    }
}
