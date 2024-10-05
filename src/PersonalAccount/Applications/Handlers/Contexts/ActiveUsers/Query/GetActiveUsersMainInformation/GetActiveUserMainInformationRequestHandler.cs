using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services;
using InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Abstracts;
using InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetActiveUsersMainInformation
{
    /// <summary>
    /// Обработчик запроса <see cref="GetActiveUserMainInformationRequest"/>
    /// </summary>
    public class GetActiveUserMainInformationRequestHandler : IQueryHandler<GetActiveUserMainInformationRequest, UserMainInformationResponse>
    {
        private readonly IReceivingActiveUsersServices _receivingActiveUsersServices;
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetActiveUserMainInformationRequestHandler"/>
        /// </summary>
        /// <param name="receivingActiveUsersServices">Сервис для получения информации о пользователях.</param>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetActiveUserMainInformationRequestHandler(
            ReceivingActiveUsersServices receivingActiveUsersServices,
            IAuthorizationData authorizationData)
        {
            _receivingActiveUsersServices = receivingActiveUsersServices;
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public Task<UserMainInformationResponse> Handle(GetActiveUserMainInformationRequest request, CancellationToken cancellationToken)
        {
            var userId = _authorizationData.UserId ?? throw new AccessDeniedException("Пользователь не авторизован");
            return _receivingActiveUsersServices.GetActiveUsersMainInformationAsync(request.UserId, cancellationToken);
        }
    }

}
