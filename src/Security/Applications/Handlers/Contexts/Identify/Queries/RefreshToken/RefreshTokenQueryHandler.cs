using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Queries.GetAuthorizationConfig;
using InsuranceGoSmoke.Security.Contracts.Identify.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Queries.RefreshToken
{
    /// <summary>
    /// Обработчик запроса <see cref="RefreshTokenQuery"/>
    /// </summary>
    public class RefreshTokenQueryHandler : IQueryHandler<RefreshTokenQuery, LoginResponse>
    {
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetAuthorizationConfigRequestHandler"/>
        /// </summary>
        /// <param name="authorizationService">Сервис работы с авторизацией.</param>
        public RefreshTokenQueryHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        /// <inheritdoc/>
        public Task<LoginResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            return _authorizationService.RefreshTokenAsync(request.RefreshToken!, cancellationToken);
        }
    }
}
