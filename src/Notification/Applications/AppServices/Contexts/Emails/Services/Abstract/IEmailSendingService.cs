namespace InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Services.Abstract
{
    /// <summary>
    /// Сервис отправки писем на email.
    /// </summary>
    public interface IEmailSendingService
    {
        /// <summary>
        /// Отправляет письмо на Email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public Task SendAsync(string email, string title, string message, CancellationToken cancellationToken);
    }
}
