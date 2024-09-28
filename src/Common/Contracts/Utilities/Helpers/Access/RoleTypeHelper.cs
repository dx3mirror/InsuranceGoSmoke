using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;

namespace InsuranceGoSmoke.Common.Contracts.Utilities.Helpers.Access
{
    /// <summary>
    /// Ошибки доступа.
    /// </summary>
    public static class RoleTypeHelper
    {
        /// <summary>
        /// Шаблон ошибки доступа.
        /// </summary>
        public static readonly string _accessDeniedErrorTextTemplate = "Операция '{0}' доступна только пользователю с ролью {1}{2}.";

        /// <summary>
        /// Возвращает текст ошибки при отказе в доступе.
        /// </summary>
        /// <param name="operation">Название операции.</param>
        /// <param name="role">Роль.</param>
        /// <returns>Текст ошибки.</returns>
        public static string GetAccessDeniedErrorText(string operation, RoleTypes role)
            => string.Format(_accessDeniedErrorTextTemplate, operation, GetRoleName(role), string.Empty);

        /// <summary>
        /// Возвращает текст ошибки при отказе в доступе.
        /// </summary>
        /// <param name="operation">Название операции.</param>
        /// <param name="role">Роль.</param>
        /// <param name="additional">Дополнение.</param>
        /// <returns>Текст ошибки.</returns>
        public static string GetAccessDeniedErrorText(string operation, RoleTypes role, string additional)
            => string.Format(_accessDeniedErrorTextTemplate, operation, GetRoleName(role), additional);

        /// <summary>
        /// Возвращает роль по названию.
        /// </summary>
        /// <param name="name">Название роли.</param>
        /// <returns>Роль.</returns>
        public static RoleTypes GetRole(string name)
        {
            return EnumHelper.GetEnumValue<RoleTypes>(name) ?? RoleTypes.Undefined;
        }

        /// <summary>
        /// Возвращает название роль.
        /// </summary>
        /// <param name="role">Роль.</param>
        /// <returns>Название роли.</returns>
        public static string GetRoleName(RoleTypes role)
        {
            return role.GetEnumDescription() ?? throw new NotFoundException("Не удалось найти наименование роли: " + role.ToString());
        }
    }
}
