using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers.Access;
using MediatR;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract
{
    /// <summary>
    /// Валидатор по текущему пользователю или роле.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    public abstract class CurrentUserOrByRoleAccessValidator<TRequest> : IAccessValidatorHandler<TRequest>
        where TRequest : IBaseRequestWithUser
    {
        /// <inheritdoc/>
        public abstract Task ValidateAsync(TRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Проверяет роль на доступ.
        /// </summary>
        /// <param name="currentUserId">Текущий пользователь.</param>
        /// <param name="userRoles">Роли пользователя.</param>
        /// <param name="request">Запрос.</param>
        /// <param name="operation">Операция.</param>
        /// <param name="role">Требуемая роль.</param>
        /// <exception cref="AccessDeniedException">Исключение, выбрасываемое, когда доступ запрещен.</exception>
        protected void Validate(Guid? currentUserId, IReadOnlyCollection<RoleTypes> userRoles, TRequest request, string operation, RoleTypes role)
        {
            if (userRoles.Contains(role))
            {
                return;
            }

            if (currentUserId == request.UserId)
            {
                return;
            }

            throw new AccessDeniedException(
                RoleTypeHelper.GetAccessDeniedErrorText(operation, role, " и пользователю, которому эти данные принадлежат"));
        }
    }
}
