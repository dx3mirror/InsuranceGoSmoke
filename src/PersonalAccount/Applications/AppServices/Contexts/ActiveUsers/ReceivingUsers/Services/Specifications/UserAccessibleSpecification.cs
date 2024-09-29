using InsuranceGoSmoke.Common.Applications.AppServices.Specifications;
using InsuranceGoSmoke.PersonalAccount.Domain.Account;

namespace InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Specifications
{
    /// <summary>
    /// Спецификация, фильтрующая пользователей с доступными аккаунтами.
    /// </summary>
    public class UserAccessibleSpecification : Specification<User>
    {
        /// <summary>
        /// Создает спецификацию для фильтрации пользователей с доступным аккаунтом.
        /// </summary>
        public UserAccessibleSpecification()
            : base(u => u.AccountStatus.IsAccessible)
        {
        }
    }
}
