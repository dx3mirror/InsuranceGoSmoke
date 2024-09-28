namespace InsuranceGoSmoke.Common.Infrastructures.DataAccess.Session
{
    /// <summary>
    /// Сессия для работы с данными.
    /// </summary>
    public interface IDataSession : IDisposable
    {
        /// <summary>
        /// Возвращает признак наличия активной транзакции.
        /// </summary>
        /// <returns>Признак наличия активной транзакции.</returns>
        bool HasActiveTransaction();

        /// <summary>
        /// Начинает выполнение транзакции.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Начинает выполнение транзакции.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены действия.</param>
        Task BeginTransactionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Подтверждает внесенные изменения.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Отменяет внесенные изменения.
        /// </summary>
        void RollbackTransaction();
    }
}
