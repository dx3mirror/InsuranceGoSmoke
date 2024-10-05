using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Abstracts;
using InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetRulePrivacyUser
{
    /// <summary>
    /// Обработчик запроса <see cref="GetUserPrivacySettingsRequest"/>
    /// </summary>
    public class GetUserPrivacySettingsRequestHandler : IQueryHandler<GetUserPrivacySettingsRequest, UserPrivacySettingsResponse>
    {
        private readonly IReceivingActiveUsersServices _receivingActiveUsersServices;
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserPrivacySettingsRequestHandler"/>
        /// </summary>
        /// <param name="receivingActiveUsersServices">Сервис для получения информации о пользователях.</param>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetUserPrivacySettingsRequestHandler(
            IReceivingActiveUsersServices receivingActiveUsersServices,
            IAuthorizationData authorizationData)
        {
            _receivingActiveUsersServices = receivingActiveUsersServices;
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public Task<UserPrivacySettingsResponse> Handle(GetUserPrivacySettingsRequest request, CancellationToken cancellationToken)
        {
            var userId = _authorizationData.UserId ?? throw new AccessDeniedException("Пользователь не авторизован");
            return _receivingActiveUsersServices.GetRulePrivacyUserAsync(request.UserId, cancellationToken);
        }
    }

}
