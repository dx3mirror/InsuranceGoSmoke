using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetUserProfileDesign
{
    /// <summary>
    /// Валидатор доступа для запроса <see cref="GetUserProfileDesignRequest"/>
    /// </summary>
    public class GetUserProfileDesignRequestAccessValidator : RoleAccessValidator<GetUserProfileDesignRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserProfileDesignRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetUserProfileDesignRequestAccessValidator(IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(GetUserProfileDesignRequest request, CancellationToken cancellationToken)
        {
            ValidateRole(_authorizationData.Roles, "Получение настроек дизайна профиля", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }
}
