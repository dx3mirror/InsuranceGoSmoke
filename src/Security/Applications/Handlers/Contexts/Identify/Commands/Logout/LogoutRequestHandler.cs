using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Commands.Logout
{
    /// <summary>
    /// Обработчик команды <see cref="LogoutRequest"/>
    /// </summary>
    public class LogoutRequestHandler : ICommandHandler<LogoutRequest>
    {
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Создаёт экземпляр <see cref="LogoutRequestHandler"/>
        /// </summary>
        /// <param name="authorizationService">Сервис авторизации.</param>
        public LogoutRequestHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        /// <inheritdoc/>
        public Task Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            return _authorizationService.LogoutAsync(cancellationToken);
        }
    }
}
