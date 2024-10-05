using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetRulePrivacyUser
{
    /// <summary>
    /// Валидатор доступа для запроса <see cref="GetUserPrivacySettingsRequest"/>
    /// </summary>
    public class GetUserPrivacySettingsRequestAccessValidator : RoleAccessValidator<GetUserPrivacySettingsRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserPrivacySettingsRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetUserPrivacySettingsRequestAccessValidator(IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(GetUserPrivacySettingsRequest request, CancellationToken cancellationToken)
        {
            ValidateRole(_authorizationData.Roles, "Получение настроек конфиденциальности пользователя", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }

}
