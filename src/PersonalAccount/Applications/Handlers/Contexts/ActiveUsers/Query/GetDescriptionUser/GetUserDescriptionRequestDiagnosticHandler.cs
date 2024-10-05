using InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DiagnosticProvider;
using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using System.Diagnostics;
using System.Reflection;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetDescriptionUser
{
    /// <summary>
    /// Обработчик диагностики запроса <see cref="GetUserDescriptionRequest"/>
    /// </summary>
    public class GetUserDescriptionRequestDiagnosticHandler : IDiagnosticHandler<GetUserDescriptionRequest>
    {
        private readonly IDiagnosticProvider _diagnosticProvider;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="diagnosticProvider">Провайдер работы с диагностикой.</param>
        public GetUserDescriptionRequestDiagnosticHandler(IDiagnosticProvider diagnosticProvider)
        {
            _diagnosticProvider = diagnosticProvider;
        }

        /// <inheritdoc/>
        public Activity? PreHandle(GetUserDescriptionRequest request)
        {
            var activity = _diagnosticProvider.StartActivity(Assembly.GetEntryAssembly());
            activity?.SetTag("correlationId", request.UserId.ToString());

            return activity;
        }

        /// <inheritdoc/>
        public void PostHandle(Activity? activity, GetUserDescriptionRequest request)
        {
            var counter = _diagnosticProvider.CreateCounter<int>("get.count", "Количество запрашиваемых описаний пользователей");
            counter?.Add(1);
        }
    }

}
