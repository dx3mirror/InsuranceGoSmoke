using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Abstracts;
using InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetUserProfileDesign
{
    /// <summary>
    /// Обработчик запроса <see cref="GetUserProfileDesignRequest"/>
    /// </summary>
    public class GetUserProfileDesignRequestHandler : IQueryHandler<GetUserProfileDesignRequest, UserProfileDesignResponse>
    {
        private readonly IReceivingActiveUsersServices _receivingActiveUsersServices;
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserProfileDesignRequestHandler"/>
        /// </summary>
        /// <param name="receivingActiveUsersServices">Сервис для получения информации о пользователях.</param>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetUserProfileDesignRequestHandler(
            IReceivingActiveUsersServices receivingActiveUsersServices,
            IAuthorizationData authorizationData)
        {
            _receivingActiveUsersServices = receivingActiveUsersServices;
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public Task<UserProfileDesignResponse> Handle(GetUserProfileDesignRequest request, CancellationToken cancellationToken)
        {
            return _receivingActiveUsersServices.GetUserProfileDesignAsync(request.UserId, cancellationToken);
        }
    }

}
