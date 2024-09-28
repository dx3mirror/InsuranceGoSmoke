using InsuranceGoSmoke.Common.Consumers.Services.Managers;
using Microsoft.Extensions.Hosting;

namespace InsuranceGoSmoke.Security.Consumers
{
    /// <summary>
    /// Сервис хостинга событий.
    /// </summary>
    public class HostingService : BackgroundService
    {
        private readonly IKafkaMessageConsumerManager _manager;

        /// <summary>
        /// Создаёт экземпляр <see cref="HostingService"/>
        /// </summary>
        /// <param name="manager">Менеджер.</param>
        public HostingService(IKafkaMessageConsumerManager manager)
        {
            _manager = manager;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _manager.StartConsumers(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
