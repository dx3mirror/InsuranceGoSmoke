using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetUserAvatar
{
    /// <summary>
    /// Валидатор доступа для запроса <see cref="GetUserAvatarRequest"/>
    /// </summary>
    public class GetUserAvatarRequestAccessValidator : RoleAccessValidator<GetUserAvatarRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserAvatarRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetUserAvatarRequestAccessValidator(IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(GetUserAvatarRequest request, CancellationToken cancellationToken)
        {
            ValidateRole(_authorizationData.Roles, "Получение аватара пользователя", RoleTypes.User);
            return Task.CompletedTask;
        }
    }
}
