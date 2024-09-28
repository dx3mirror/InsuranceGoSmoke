using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Paged;
using InsuranceGoSmoke.Common.Hosts.Api.Filters.RateLimit;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.ChangePhoneNumber;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.ChangeUserRole;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.EmailVerification;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SendEmailVerificationCode;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SetStatusUser;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUser;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUserField;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUser;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPaged;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPagedByQuery;
using InsuranceGoSmoke.Security.Contracts.Identify.Options;
using InsuranceGoSmoke.Security.Contracts.Users.Requests;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace InsuranceGoSmoke.Security.Public.Hosts.Api.Controllers
{
    /// <summary>
    /// API работы с пользователем.
    /// </summary>
    /// <param name="_mediator">Обработчик сообщений.</param>
    /// <param name="_options">Настроки идентификации.</param>
    [Route("user")]
    [ApiController]
    public class UserController(IMediator _mediator, IOptions<IdentifyOptions> _options) : ControllerBase
    {
        /// <summary>
        /// Возвращает список пользователей.
        /// </summary>
        /// <param name="filter">Фильтр.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список пользователей.</returns>
        [HttpGet("/users")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(PagedList<UserPagedListItem>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetUsers([FromQuery, BindRequired] UsersPagedByQueryFilter filter, CancellationToken cancellationToken)
        {
            var request = new GetUsersPagedByQueryRequest(filter.Query, filter.Take, filter.Skip);
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Возвращает список пользователей.
        /// </summary>
        /// <param name="filter">Фильтр.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список пользователей.</returns>
        [HttpPost("/users")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(PagedList<UserPagedListItem>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetUsers([FromBody, BindRequired] UsersPagedFilter filter, CancellationToken cancellationToken)
        {
            var request = new GetUsersPagedRequest(filter);
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Возвращает данные пользователя.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId, CancellationToken cancellationToken)
        {
            var request = new GetUserRequest(userId);
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Изменяет данные пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="user">Данные пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UserData user, CancellationToken cancellationToken)
        {
            var request = new UpdateUserRequest(userId, user);
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }


        /// <summary>
        /// Изменяет поле данных пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="field">Изменяемое поле.</param>
        /// <param name="value">Значение.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpPatch("{userId}/{field}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<IActionResult> UpdateUserField([FromRoute] Guid userId, 
            [FromRoute] string field, [FromBody, BindRequired] JsonElement value, CancellationToken cancellationToken)
        {
            var request = new UpdateUserFieldRequest(userId, field, value);
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Изменяет статус пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="status">Данные о статусе пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpPut("{userId}/status")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<IActionResult> SetStatusUser([FromRoute, BindRequired] Guid userId, [FromBody, BindRequired] SetUserStatusData status, 
            CancellationToken cancellationToken)
        {
            var request = new SetStatusUserRequest(userId, status.IsEnabled);
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Меняет номер телефона.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpPut("{userId}/phone/{phoneNumber}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<IActionResult> UpdatePhoneNumberAsync([FromRoute, BindRequired] Guid userId,
            [FromRoute, BindRequired] string phoneNumber, CancellationToken cancellationToken)
        {
            var request = new UpdatePhoneNumberRequest(userId, phoneNumber);
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Отправляет код для подтверждения Email.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="email">Email.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpPost("{userId}/email/verify")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [RateLimit(maxAttempts: 1, delaySeconds: 179, uniqueParameter: nameof(email))] // 1 раз в 3 минуты
        [Authorize]
        public async Task<IActionResult> SendEmailVerificationCode([FromRoute, BindRequired] Guid userId, [FromQuery, BindRequired] string email,
            CancellationToken cancellationToken)
        {
            var request = new SendEmailVerificationCodeRequest(userId, email);
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Подтверждает Email.
        /// </summary>
        /// <param name="code">Код верификации.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpGet("email/verify")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [AllowAnonymous]
        public async Task<IActionResult> EmailVerification([BindRequired] string code, 
            CancellationToken cancellationToken)
        {
            var request = new EmailVerificationRequest(code);
            await _mediator.Send(request, cancellationToken);
            return Redirect(_options.Value.EmailVerificationRedirectUrl);
        }

        /// <summary>
        /// Изменяет роль пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="role">Роль.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpPut("{userId}/role/{role}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<IActionResult> ChangeUserRole([FromRoute, BindRequired] Guid userId, [FromRoute, BindRequired] RoleTypes role,
            CancellationToken cancellationToken)
        {
            var request = new ChangeUserRoleRequest(userId, role);
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }
    }
}
