using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetDescriptionUser
{
    /// <summary>
    /// Валидатор доступа для запроса <see cref="GetUserDescriptionRequest"/>
    /// </summary>
    public class GetUserDescriptionRequestAccessValidator : RoleAccessValidator<GetUserDescriptionRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserDescriptionRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetUserDescriptionRequestAccessValidator(IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(GetUserDescriptionRequest request, CancellationToken cancellationToken)
        {
            ValidateRole(_authorizationData.Roles, "Получение описания пользователя", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }
}
