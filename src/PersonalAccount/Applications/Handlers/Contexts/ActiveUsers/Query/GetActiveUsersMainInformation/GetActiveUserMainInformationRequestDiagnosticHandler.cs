using InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DiagnosticProvider;
using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using System.Diagnostics;
using System.Reflection;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetActiveUsersMainInformation
{
    /// <summary>
    /// Обработчик диагностики запроса <see cref="GetActiveUserMainInformationRequest"/>
    /// </summary>
    public class GetActiveUserMainInformationRequestDiagnosticHandler : IDiagnosticHandler<GetActiveUserMainInformationRequest>
    {
        private readonly IDiagnosticProvider _diagnosticProvider;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="diagnosticProvider">Провайдер работы с диагностикой.</param>
        public GetActiveUserMainInformationRequestDiagnosticHandler(IDiagnosticProvider diagnosticProvider)
        {
            _diagnosticProvider = diagnosticProvider;
        }

        /// <inheritdoc/>
        public Activity? PreHandle(GetActiveUserMainInformationRequest request)
        {
            var activity = _diagnosticProvider.StartActivity(Assembly.GetEntryAssembly());
            activity?.SetTag("correlationId", request.UserId.ToString());

            return activity;
        }

        /// <inheritdoc/>
        public void PostHandle(Activity? activity, GetActiveUserMainInformationRequest request)
        {
            var counter = _diagnosticProvider.CreateCounter<int>("get.count", "Количество запрашиваемой основной информации о пользователе");
            counter?.Add(1);
        }
    }
}
