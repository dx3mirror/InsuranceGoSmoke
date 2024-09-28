using MediatR;
using System.Diagnostics;

namespace InsuranceGoSmoke.Common.Applications.Handlers.Abstract
{
    /// <summary>
    /// Интерфейс обработчика по диагносике.
    /// </summary>
    public interface IDiagnosticHandler<in TRequest>
        where TRequest : IBaseRequest
    {
        /// <summary>
        /// Предобработка.
        /// </summary>
        /// <param name="request">Запрос.</param>
        public Activity? PreHandle(TRequest request);

        /// <summary>
        /// Постобработка.
        /// </summary>
        /// <param name="actiity">Активность.</param>
        /// <param name="request">Запрос.</param>
        public void PostHandle(Activity? actiity, TRequest request);
    }

    /// <summary>
    /// Интерфейс обработчика по диагносике.
    /// </summary>
    public interface IDiagnosticHandler<in TRequest, in TResponse>
        where TRequest : IBaseRequest
    {
        /// <summary>
        /// Предобработка.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Активность.</returns>
        public Activity? PreHandle(TRequest request);

        /// <summary>
        /// Постобработка.
        /// </summary>
        /// <param name="actiity">Активность.</param>
        /// <param name="request">Запрос.</param>
        /// <param name="response">Запрос.</param>
        public void PostHandle(Activity? actiity, TRequest request, TResponse response);
    }
}
