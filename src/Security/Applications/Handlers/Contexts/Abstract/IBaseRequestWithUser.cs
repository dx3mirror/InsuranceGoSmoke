using MediatR;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract
{
    /// <summary>
    /// Базовый запрос с идентификатором о пользователе.
    /// </summary>
    public interface IBaseRequestWithUser : IBaseRequest
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; }
    }
}
