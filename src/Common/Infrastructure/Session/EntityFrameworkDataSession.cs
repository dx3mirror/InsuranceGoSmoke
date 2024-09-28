using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace InsuranceGoSmoke.Common.Infrastructures.DataAccess.Session
{
    /// <summary>
    /// Сессия данных EntityFramework.
    /// </summary>
    /// <param name="context">контекст.</param>
    public sealed class EntityFrameworkDataSession(DbContext context) : IDataSession
    {
        /// <inheritdoc />
        public bool HasActiveTransaction() 
            => GetCurrentTransaction(context) != null;

        /// <inheritdoc />
        public void BeginTransaction() 
            => context.Database.BeginTransaction();

        /// <inheritdoc />
        public async Task BeginTransactionAsync(CancellationToken cancellationToken) 
            => await context.Database.BeginTransactionAsync(cancellationToken);

        /// <inheritdoc />
        public void CommitTransaction() 
            => GetCurrentTransaction(context)?.Commit();

        /// <inheritdoc />
        public void RollbackTransaction() 
            => GetCurrentTransaction(context)?.Rollback();

        /// <inheritdoc />
        public void Dispose() 
            => RollbackTransaction();

        private static IDbContextTransaction? GetCurrentTransaction(DbContext context) 
            => context.Database.CurrentTransaction;
    }
}
