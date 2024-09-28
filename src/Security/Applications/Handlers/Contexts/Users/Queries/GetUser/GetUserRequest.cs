using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUser
{
    /// <summary>
    /// Запрос на получение данных пользователя.
    /// </summary>
    public class GetUserRequest : Query<UserResponse>, IBaseRequestWithUser
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="UserResponse"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public GetUserRequest(Guid userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; }
    }
}
