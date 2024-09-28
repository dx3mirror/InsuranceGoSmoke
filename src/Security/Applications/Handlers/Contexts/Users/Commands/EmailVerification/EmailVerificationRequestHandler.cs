using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.EmailVerification
{
    /// <summary>
    /// Обработчик команды <see cref="EmailVerificationRequest"/>
    /// </summary>
    public class EmailVerificationRequestHandler : ICommandHandler<EmailVerificationRequest>
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Создаёт экземпляр <see cref="EmailVerificationRequestHandler"/>
        /// </summary>
        /// <param name="userService">Сервис работы с пользователем.</param>
        public EmailVerificationRequestHandler(
            IUserService userService)
        {
            _userService = userService;
        }

        /// <inheritdoc/>
        public Task Handle(EmailVerificationRequest request, CancellationToken cancellationToken)
        {
            return _userService.EmailVerificationAsync(request.Code, cancellationToken);
        }
    }
}
