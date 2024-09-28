using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;

namespace InsuranceGoSmoke.Common.Contracts.Contracts.Authorization
{
    /// <inheritdoc/>
    public class AuthorizationData : IAuthorizationData
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="AuthorizationData"/>
        /// </summary>
        public AuthorizationData()
        {
        }

        /// <summary>
        /// Создаёт экземпляр <see cref="AuthorizationData"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public AuthorizationData(Guid userId) : this()
        {
            UserId = userId;
        }

        /// <inheritdoc/>
        public Guid? UserId { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<RoleTypes> Roles { get; set; } = [];
    }
}
