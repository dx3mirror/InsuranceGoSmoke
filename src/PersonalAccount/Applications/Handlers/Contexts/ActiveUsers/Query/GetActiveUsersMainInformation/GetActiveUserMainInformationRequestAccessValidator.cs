using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetActiveUsersMainInformation
{
    /// <summary>
    /// Валидатор доступа для запроса <see cref="GetActiveUserMainInformationRequest"/>
    /// </summary>
    public class GetActiveUserMainInformationRequestAccessValidator : RoleAccessValidator<GetActiveUserMainInformationRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetActiveUserMainInformationRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetActiveUserMainInformationRequestAccessValidator(IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(GetActiveUserMainInformationRequest request, CancellationToken cancellationToken)
        {
            ValidateRole(_authorizationData.Roles, "Получение основной информации о пользователе", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }

}
