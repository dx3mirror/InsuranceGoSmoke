using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Factories.Strategies;

namespace InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Factories
{
    /// <summary>
    /// Фабрика стратегий отправки письма на Email.
    /// </summary>
    public interface IEmailSenderStrategiesFactory
    {
        /// <summary>
        /// Возвращает стратегию отправки Email.
        /// </summary>
        /// <returns>Стратегия отправки Email.</returns>
        IEmailSenderStrategy GetStrategy();
    }
}
