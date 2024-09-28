using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Paged;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPaged;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPagedByQuery
{
    /// <summary>
    /// Обработчик команды <see cref="GetUsersPagedByQueryRequest"/>
    /// </summary>
    public class GetUsersPagedByQueryRequestHandler : IQueryHandler<GetUsersPagedByQueryRequest, PagedList<UserPagedListItem>>
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUsersPagedRequestHandler"/>
        /// </summary>
        /// <param name="userService">Сервис работы с пользователем.</param>
        public GetUsersPagedByQueryRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        /// <inheritdoc/>
        public Task<PagedList<UserPagedListItem>> Handle(GetUsersPagedByQueryRequest request, CancellationToken cancellationToken)
        {
            return _userService.GetPagedUsersByQueryAsync(request.Query, request.Take, request.Skip, cancellationToken);
        }
    }
}
