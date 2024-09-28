using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Dictionaries.Queries.GetRoles;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceGoSmoke.Security.Public.Hosts.Api.Controllers
{
    /// <summary>
    /// Контроллер справочников.
    /// </summary>
    /// <param name="_mediator">Обработчик сообщений.</param>
    [Route("dictionary")]
    [ApiController]
    public class DictionaryController(IMediator _mediator) : ControllerBase
    {
        /// <summary>
        /// Возвращает конфигурацию авторизации.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Конфигурация авторизации.</returns>
        [HttpGet("roles")]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(IReadOnlyCollection<RoleResponse>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            var request = new GetRolesRequest();
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }
    }
}
