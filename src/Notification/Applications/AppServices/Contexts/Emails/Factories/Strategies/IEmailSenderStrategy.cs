namespace InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Factories.Strategies
{
    /// <summary>
    /// Стратегия отправки письма на Email.
    /// </summary>
    public interface IEmailSenderStrategy
    {
        /// <summary>
        /// Отправляет письмо на email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task SendAsync(string email, string title, string message, CancellationToken cancellationToken);
    }
}
