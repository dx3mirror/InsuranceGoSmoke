using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUser;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SetStatusUser
{
    /// <summary>
    /// Обработчик команды <see cref="SetStatusUserRequest"/>
    /// </summary>
    public class SetStatusUserRequestHandler : ICommandHandler<SetStatusUserRequest>
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Создаёт экземпляр <see cref="UpdateUserRequestHandler"/>
        /// </summary>
        /// <param name="userService">Сервис работы с пользователем.</param>
        public SetStatusUserRequestHandler(
            IUserService userService)
        {
            _userService = userService;
        }

        /// <inheritdoc/>
        public Task Handle(SetStatusUserRequest request, CancellationToken cancellationToken)
        {
            return _userService.SetStatusUserAsync(request.UserId, request.IsEnabled ?? false, cancellationToken);
        }
    }
}
