using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Events;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Events.EmailConfirmNotification;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SendEmailVerificationCode
{
    /// <summary>
    /// Обработчик команды <see cref="SendEmailVerificationCodeRequest"/>
    /// </summary>
    public class SendEmailVerificationCodeRequestHandler : ICommandHandler<SendEmailVerificationCodeRequest>
    {
        private readonly IEventMessageProvider _eventMessageProvider;

        /// <summary>
        /// Создаёт экземпляр <see cref="SendEmailVerificationCodeRequestHandler"/>
        /// </summary>
        /// <param name="eventMessageProvider">Провайдер событий.</param>
        public SendEmailVerificationCodeRequestHandler(IEventMessageProvider eventMessageProvider)
        {
            _eventMessageProvider = eventMessageProvider;
        }

        /// <inheritdoc/>
        public Task Handle(SendEmailVerificationCodeRequest request, CancellationToken cancellationToken)
        {
            _eventMessageProvider.Add(new EmailConfirmNotificationEvent(request.UserId, request.Email));
            return Task.CompletedTask;
        }
    }
}
