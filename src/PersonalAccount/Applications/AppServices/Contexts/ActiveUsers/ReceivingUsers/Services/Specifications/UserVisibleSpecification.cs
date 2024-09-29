using InsuranceGoSmoke.Common.Applications.AppServices.Specifications;
using InsuranceGoSmoke.PersonalAccount.Domain.Account;

namespace InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Specifications
{
    /// <summary>
    /// Спецификация, фильтрующая пользователей с видимым аккаунтом.
    /// </summary>
    public class UserVisibleSpecification : Specification<User>
    {
        /// <summary>
        /// Создает спецификацию для фильтрации пользователей с видимыми аккаунтами.
        /// </summary>
        public UserVisibleSpecification()
            : base(u => u.AccountStatus.IsVisible)
        {
        }
    }
}
