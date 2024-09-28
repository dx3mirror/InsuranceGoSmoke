using InsuranceGoSmoke.Common.Contracts.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Events.EmailConfirmNotification
{
    /// <summary>
    /// Событие уведомления о необходимости подтвердить Email.
    /// </summary>
    public class EmailConfirmNotificationEvent : Event
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="EmailConfirmNotificationEvent"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="email">Email.</param>
        public EmailConfirmNotificationEvent(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; }
    }
}
