using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Abstracts;
using InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetUserAvatar
{
    /// <summary>
    /// Обработчик запроса <see cref="GetUserAvatarRequest"/>
    /// </summary>
    public class GetUserAvatarRequestHandler : IQueryHandler<GetUserAvatarRequest, UserAvatarResponse>
    {
        private readonly IReceivingActiveUsersServices _receivingActiveUsersServices;
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserAvatarRequestHandler"/>
        /// </summary>
        /// <param name="receivingActiveUsersServices">Сервис для получения информации о пользователях.</param>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetUserAvatarRequestHandler(
            IReceivingActiveUsersServices receivingActiveUsersServices,
            IAuthorizationData authorizationData)
        {
            _receivingActiveUsersServices = receivingActiveUsersServices;
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public Task<UserAvatarResponse> Handle(GetUserAvatarRequest request, CancellationToken cancellationToken)
        {
            return _receivingActiveUsersServices.GetUserAvatarAsync(request.UserId, cancellationToken);
        }
    }
}
