using InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DiagnosticProvider;
using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using System.Diagnostics;
using System.Reflection;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetRulePrivacyUser
{
    /// <summary>
    /// Обработчик диагностики запроса <see cref="GetUserPrivacySettingsRequest"/>
    /// </summary>
    public class GetUserPrivacySettingsRequestDiagnosticHandler : IDiagnosticHandler<GetUserPrivacySettingsRequest>
    {
        private readonly IDiagnosticProvider _diagnosticProvider;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="diagnosticProvider">Провайдер работы с диагностикой.</param>
        public GetUserPrivacySettingsRequestDiagnosticHandler(IDiagnosticProvider diagnosticProvider)
        {
            _diagnosticProvider = diagnosticProvider;
        }

        /// <inheritdoc/>
        public Activity? PreHandle(GetUserPrivacySettingsRequest request)
        {
            var activity = _diagnosticProvider.StartActivity(Assembly.GetEntryAssembly());
            activity?.SetTag("correlationId", request.UserId.ToString());

            return activity;
        }

        /// <inheritdoc/>
        public void PostHandle(Activity? activity, GetUserPrivacySettingsRequest request)
        {
            var counter = _diagnosticProvider.CreateCounter<int>("get.count", "Количество запрашиваемых настроек конфиденциальности");
            counter?.Add(1);
        }
    }

}
