using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers.Access;
using MediatR;

namespace InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator
{
    /// <summary>
    /// Валидатор по роли.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    public abstract class RoleAccessValidator<TRequest> : IAccessValidatorHandler<TRequest>
        where TRequest : IBaseRequest
    {
        /// <inheritdoc/>
        public abstract Task ValidateAsync(TRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Проверяет роль на доступ.
        /// </summary>
        /// <param name="userRoles">Роли пользователя.</param>
        /// <param name="operation">Операция.</param>
        /// <param name="role">Требуемая роль.</param>
        /// <exception cref="AccessDeniedException">Исключение, выбрасываемое, когда доступ запрещен.</exception>
        protected void ValidateRole(IReadOnlyCollection<RoleTypes> userRoles, string operation, RoleTypes role)
        {
            if (userRoles.Contains(role))
            {
                return;
            }

            throw new AccessDeniedException(
                RoleTypeHelper.GetAccessDeniedErrorText(operation, role));
        }
    }
}
