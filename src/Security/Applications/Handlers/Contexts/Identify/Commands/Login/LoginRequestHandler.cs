using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services.Abstract;
using InsuranceGoSmoke.Security.Contracts.Identify.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Commands.Login
{
    /// <summary>
    /// Обработчик команды <see cref="LoginRequest"/>
    /// </summary>
    public class LoginRequestHandler : ICommandHandler<LoginRequest, LoginResponse>
    {
        private readonly IAuthorizationService _aurthorizationService;

        /// <summary>
        /// Создаёт экземпляр <see cref="LoginRequestHandler"/>
        /// </summary>
        /// <param name="aurthorizationService">Сервис работы с авторизацией.</param>
        public LoginRequestHandler(IAuthorizationService aurthorizationService)
        {
            _aurthorizationService = aurthorizationService;
        }

        /// <inheritdoc/>
        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var response = await _aurthorizationService.AuthorizationAsync(request.UserName, request.Password, cancellationToken);
            return response;
        }
    }
}
