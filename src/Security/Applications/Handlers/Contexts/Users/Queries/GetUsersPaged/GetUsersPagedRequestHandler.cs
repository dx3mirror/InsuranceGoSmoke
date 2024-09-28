using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Paged;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Models;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUser;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPaged;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;

namespace Kortros.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPaged
{
    /// <summary>
    /// Обработчик команды <see cref="GetUserRequest"/>
    /// </summary>
    public class GetUsersPagedRequestHandler : IQueryHandler<GetUsersPagedRequest, PagedList<UserPagedListItem>>
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUsersPagedRequestHandler"/>
        /// </summary>
        /// <param name="userService">Сервис работы с пользователем.</param>
        public GetUsersPagedRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        /// <inheritdoc/>
        public Task<PagedList<UserPagedListItem>> Handle(GetUsersPagedRequest request, CancellationToken cancellationToken)
        {
            var data = new GetPagedUsersModel(request.Take, request.Skip)
            {
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                LastName = request.LastName,
                FirstName = request.FirstName,
                Role = request.Role,
                IsEnabled = request.IsEnabled
            };
            return _userService.GetPagedUsersAsync(data, cancellationToken);
        }
    }
}
