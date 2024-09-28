using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;

namespace InsuranceGoSmoke.Common.Contracts.Contracts.Authorization
{
    /// <summary>
    /// Авторизационные данные.
    /// </summary>
    public interface IAuthorizationData
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid? UserId { get; }

        /// <summary>
        /// Роли.
        /// </summary>
        public IReadOnlyCollection<RoleTypes> Roles { get; set; }

        /// <summary>
        /// Признак, что пользователь - администратор.
        /// </summary>
        public bool IsAdmin => Roles.Any(r => r == RoleTypes.Administrator);
    }
}
