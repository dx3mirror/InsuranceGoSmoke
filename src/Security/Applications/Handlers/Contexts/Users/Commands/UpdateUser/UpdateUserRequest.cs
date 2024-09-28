using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract;
using InsuranceGoSmoke.Security.Contracts.Users.Enums;
using InsuranceGoSmoke.Security.Contracts.Users.Requests;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUser
{
    /// <summary>
    /// Команда на обновление данных пользователя.
    /// </summary>
    public class UpdateUserRequest : Command, IBaseRequestWithUser
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="UpdateUserRequest" />
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="data">Данные.</param>
        public UpdateUserRequest(Guid userId, UserData data)
        {
            UserId = userId;
            FirstName = data.FirstName;
            LastName = data.LastName;
            Sex = data.Sex;
            Email = data.Email;
            BirthDate = data.BirthDate;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string? FirstName { get; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string? LastName { get; }

        /// <summary>
        /// Пол.
        /// </summary>
        public SexType? Sex { get; set; }

        /// <summary>
        /// День рождения.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; }
    }
}
