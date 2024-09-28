using MediatR;

namespace InsuranceGoSmoke.Common.Applications.Handlers.Abstract
{
    /// <summary>
    /// Обработчик команды.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
    }

    /// <summary>
    /// Обработчик команды.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
    }
}
