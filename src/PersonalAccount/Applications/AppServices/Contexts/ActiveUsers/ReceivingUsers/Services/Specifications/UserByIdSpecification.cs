using InsuranceGoSmoke.Common.Applications.AppServices.Specifications;
using InsuranceGoSmoke.PersonalAccount.Domain.Account;

namespace InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Specifications
{
    /// <summary>
    /// Спецификация, фильтрующая пользователей по их идентификатору (ID).
    /// </summary>
    public class UserByIdSpecification : Specification<User>
    {
        /// <summary>
        /// Создает спецификацию для фильтрации пользователя по идентификатору (ID).
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public UserByIdSpecification(long userId)
            : base(u => u.Id == userId)
        {
        }
    }

}
