namespace InsuranceGoSmoke.Common.Consumers.Services.Managers
{
    /// <summary>
    /// Менеджер для работы с consumer'ами.
    /// </summary>
    public interface IKafkaMessageConsumerManager
    {
        /// <summary>
        /// Запускает работу consumer'ов.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        void StartConsumers(CancellationToken cancellationToken);
    }
}
