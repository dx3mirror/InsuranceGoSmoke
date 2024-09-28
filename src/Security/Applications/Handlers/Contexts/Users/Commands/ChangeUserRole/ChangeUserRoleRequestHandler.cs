using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.ChangeUserRole
{
    /// <summary>
    /// Обработчик команды <see cref="ChangeUserRoleRequest"/>
    /// </summary>
    public class ChangeUserRoleRequestHandler : ICommandHandler<ChangeUserRoleRequest>
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Создаёт экземпляр <see cref="ChangeUserRoleRequestHandler"/>
        /// </summary>
        /// <param name="userService">Сервис работы с пользователем.</param>
        public ChangeUserRoleRequestHandler(
            IUserService userService)
        {
            _userService = userService;
        }

        /// <inheritdoc/>
        public Task Handle(ChangeUserRoleRequest request, CancellationToken cancellationToken)
        {
            return _userService.ChangeUserRoleAsync(request.UserId, request.Role, cancellationToken);
        }
    }
}
