using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUser;
using System.Text.Json;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUserField
{
    /// <summary>
    /// Запрос на обновление поля данных пользователя.
    /// </summary>
    public class UpdateUserFieldRequest : Command, IBaseRequestWithUser
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="UpdateUserRequest" />
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="field">Поле.</param>
        /// <param name="value">Значение.</param>
        public UpdateUserFieldRequest(Guid userId, string field, JsonElement value)
        {
            UserId = userId;
            Field = field;
            Value = value;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Поле.
        /// </summary>
        public string Field { get; }

        /// <summary>
        /// Значение.
        /// </summary>
        public JsonElement Value { get; }
    }
}
