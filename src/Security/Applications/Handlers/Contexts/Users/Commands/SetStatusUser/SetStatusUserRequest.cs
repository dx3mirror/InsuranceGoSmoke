using InsuranceGoSmoke.Common.Contracts.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SetStatusUser
{
    /// <summary>
    /// Запрос на изменение статуса пользователя.
    /// </summary>
    public class SetStatusUserRequest : Command
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="SetStatusUserRequest"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="isEnabled">Признак, что пользователь активен.</param>
        public SetStatusUserRequest(Guid userId, bool? isEnabled)
        {
            UserId = userId;
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Признак, что пользователь активен.
        /// </summary>
        public bool? IsEnabled { get; }
    }
}
