using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Commands.SendAuthentificationCode
{
    /// <summary>
    /// Обработчик команды <see cref="SendAuthentificationCodeRequest"/>
    /// </summary>
    public class SendAuthentificationCodeRequestHandler : ICommandHandler<SendAuthentificationCodeRequest>
    {
        private readonly IAuthorizationService _aurthorizationService;

        /// <summary>
        /// Создаёт экземпляр <see cref="SendAuthentificationCodeRequestHandler"/>
        /// </summary>
        /// <param name="aurthorizationService">Сервис работы с авторизацией.</param>
        public SendAuthentificationCodeRequestHandler(IAuthorizationService aurthorizationService)
        {
            _aurthorizationService = aurthorizationService;
        }

        /// <inheritdoc/>
        public Task Handle(SendAuthentificationCodeRequest request, CancellationToken cancellationToken)
        {
            return _aurthorizationService.SendAuthentificationCodeAsync(request.Destination, cancellationToken);
        }
    }
}
