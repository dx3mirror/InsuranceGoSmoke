using System.ComponentModel;

namespace InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums
{
    /// <summary>
    /// Роли.
    /// </summary>
    public enum RoleTypes
    {
        /// <summary>
        /// Неопределенно.
        /// </summary>
        Undefined,

        /// <summary>
        /// Администратор.
        /// </summary>
        [Description("Администратор")]
        Administrator,

        /// <summary>
        /// Клиент.
        /// </summary>
        [Description("Клиент")]
        Client,
    }
}
