using MediatR;

namespace InsuranceGoSmoke.Common.Applications.Handlers.Abstract
{
    /// <summary>
    /// Обработчик команды.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
    }
}
