namespace InsuranceGoSmoke.Common.Hosts.Migrations.Abstracts
{
    /// <summary>
    /// Интерфейс для выполнения операций по применению Seed'ов.
    /// </summary>
    public interface ISeedExecutor
    {
        /// <summary>
        /// Выполняет операции по применению Seed'ов.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены для возможности отмены выполнения операции.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task RunAsync(CancellationToken cancellationToken);
    }
}
