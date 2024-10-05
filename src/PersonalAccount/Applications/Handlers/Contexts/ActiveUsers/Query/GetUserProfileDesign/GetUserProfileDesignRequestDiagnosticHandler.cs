using InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DiagnosticProvider;
using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using System.Diagnostics;
using System.Reflection;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetUserProfileDesign
{
    /// <summary>
    /// Обработчик диагностики запроса <see cref="GetUserProfileDesignRequest"/>
    /// </summary>
    public class GetUserProfileDesignRequestDiagnosticHandler : IDiagnosticHandler<GetUserProfileDesignRequest>
    {
        private readonly IDiagnosticProvider _diagnosticProvider;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="diagnosticProvider">Провайдер работы с диагностикой.</param>
        public GetUserProfileDesignRequestDiagnosticHandler(IDiagnosticProvider diagnosticProvider)
        {
            _diagnosticProvider = diagnosticProvider;
        }

        /// <inheritdoc/>
        public Activity? PreHandle(GetUserProfileDesignRequest request)
        {
            var activity = _diagnosticProvider.StartActivity(Assembly.GetEntryAssembly());
            activity?.SetTag("correlationId", request.UserId.ToString());

            return activity;
        }

        /// <inheritdoc/>
        public void PostHandle(Activity? activity, GetUserProfileDesignRequest request)
        {
            var counter = _diagnosticProvider.CreateCounter<int>("get.count", "Количество запрашиваемых настроек дизайна профиля пользователя");
            counter?.Add(1);
        }
    }
}
