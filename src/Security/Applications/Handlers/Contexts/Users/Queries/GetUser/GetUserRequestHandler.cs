using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUser
{
    /// <summary>
    /// Обработчик команды <see cref="GetUserRequest"/>
    /// </summary>
    public class GetUserRequestHandler : ICommandHandler<GetUserRequest, UserResponse>
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserRequestHandler"/>
        /// </summary>
        /// <param name="userService">Сервис работы с пользователем.</param>
        public GetUserRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        /// <inheritdoc/>
        public Task<UserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            return _userService.GetUserAsync(request.UserId, cancellationToken);
        }
    }
}
