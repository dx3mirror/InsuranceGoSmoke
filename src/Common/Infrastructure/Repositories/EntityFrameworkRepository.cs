using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InsuranceGoSmoke.Common.Infrastructures.DataAccess.Repositories
{
    /// <inheritdoc/>
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        protected DbContext DbContext { get; }

        /// <summary>
        /// Хранилище сущностей <typeparamref name="TEntity"/>.
        /// </summary>
        protected DbSet<TEntity> DbSet { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public EntityFrameworkRepository(DbContext context)
        {
            DbContext = context;
            DbSet = DbContext.Set<TEntity>();
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> AsQueryable()
        {
            return DbSet.AsQueryable<TEntity>();
        }

        /// <inheritdoc/>
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);
            
            await DbContext.AddAsync(entity, cancellationToken);

            await SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddRangeAsync(TEntity[] entities, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entities);

            await DbContext.AddRangeAsync(entities, cancellationToken);

            await SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);
            return DbSet.Where(expression);
        }

        /// <inheritdoc />
        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var state = DbContext.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                DbContext.Attach(entity);
            }

            DbContext.Update(entity);

            return SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (var entity in entities)
            {
                DbContext.Remove(entity);
            }

            await SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task ExecuteRawSqlAsync(string sql, IReadOnlyCollection<object> parameters, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(sql);

            DbContext.Database.SqlQueryRaw<TEntity>(sql, parameters);

            return SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        private Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return await DbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            await transaction.CommitAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            await transaction.RollbackAsync(cancellationToken);
        }
    }
}
