using InsuranceGoSmoke.Common.Applications.AppServices.Specifications;
using InsuranceGoSmoke.PersonalAccount.Domain.Account;

namespace InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Specifications
{
    /// <summary>
    /// Спецификация, фильтрующая пользователей, чей аккаунт не заблокирован.
    /// </summary>
    public class UserNotBlockedSpecification : Specification<User>
    {
        /// <summary>
        /// Создает спецификацию для фильтрации пользователей с незаблокированными аккаунтами.
        /// </summary>
        public UserNotBlockedSpecification()
            : base(u => !u.AccountStatus.IsBlocked)
        {
        }
    }
}
