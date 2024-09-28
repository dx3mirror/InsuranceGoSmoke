using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;

namespace InsuranceGoSmoke.Security.Contracts.Users.Responses
{
    /// <summary>
    /// Роль.
    /// </summary>
    public class RoleResponse
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="RoleResponse"/>
        /// </summary>
        /// <param name="type">Тип роли.</param>
        /// <param name="name">Наименование.</param>
        public RoleResponse(RoleTypes type, string name)
        {
            Type = type;
            Name = name;
        }

        /// <summary>
        /// Тип роли.
        /// </summary>
        public RoleTypes Type { get; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; }
    }
}
