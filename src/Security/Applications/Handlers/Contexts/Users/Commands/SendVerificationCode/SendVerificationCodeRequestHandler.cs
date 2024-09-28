using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SendVerificationCode
{
    /// <summary>
    /// Обработчик команды <see cref="SendVerificationCodeRequest"/>
    /// </summary>
    public class SendVerificationCodeRequestHandler : ICommandHandler<SendVerificationCodeRequest>
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Создаёт экземпляр <see cref="SendVerificationCodeRequestHandler"/>
        /// </summary>
        /// <param name="userService">Сервис работы с пользователем.</param>
        public SendVerificationCodeRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        /// <inheritdoc/>
        public Task Handle(SendVerificationCodeRequest request, CancellationToken cancellationToken)
        {
            return _userService.SendVerificationCodeAsync(request.PhoneNumber, cancellationToken);
        }
    }
}
