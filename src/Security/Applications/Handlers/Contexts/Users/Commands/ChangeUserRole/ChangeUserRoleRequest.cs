using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.ChangeUserRole
{
    /// <summary>
    /// Команда на изменение роли пользователя.
    /// </summary>
    public class ChangeUserRoleRequest : Command
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="ChangeUserRoleRequest"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="role">Роль.</param>
        public ChangeUserRoleRequest(Guid userId, RoleTypes role)
        {
            UserId = userId;
            Role = role;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Признак, что пользователь активен.
        /// </summary>
        public RoleTypes Role { get; }
    }
}
