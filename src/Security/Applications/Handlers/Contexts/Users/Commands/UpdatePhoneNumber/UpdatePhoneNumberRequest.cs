using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.ChangePhoneNumber
{
    /// <summary>
    /// Команда на изменение номера телефона.
    /// </summary>
    public class UpdatePhoneNumberRequest : Command, IBaseRequestWithUser
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="UpdatePhoneNumberRequest"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="phoneNumber">Номер телефона.</param>
        public UpdatePhoneNumberRequest(Guid userId, string phoneNumber)
        {
            UserId = userId;
            PhoneNumber = phoneNumber;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string PhoneNumber { get; }
    }
}
