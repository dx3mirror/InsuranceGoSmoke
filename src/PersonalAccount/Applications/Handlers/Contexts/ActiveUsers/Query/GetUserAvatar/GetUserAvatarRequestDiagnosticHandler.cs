using InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DiagnosticProvider;
using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using System.Diagnostics;
using System.Reflection;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetUserAvatar
{
    /// <summary>
    /// Обработчик диагностики запроса <see cref="GetUserAvatarRequest"/>
    /// </summary>
    public class GetUserAvatarRequestDiagnosticHandler : IDiagnosticHandler<GetUserAvatarRequest>
    {
        private readonly IDiagnosticProvider _diagnosticProvider;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="diagnosticProvider">Провайдер работы с диагностикой.</param>
        public GetUserAvatarRequestDiagnosticHandler(IDiagnosticProvider diagnosticProvider)
        {
            _diagnosticProvider = diagnosticProvider;
        }

        /// <inheritdoc/>
        public Activity? PreHandle(GetUserAvatarRequest request)
        {
            var activity = _diagnosticProvider.StartActivity(Assembly.GetEntryAssembly());
            activity?.SetTag("correlationId", request.UserId.ToString());

            return activity;
        }

        /// <inheritdoc/>
        public void PostHandle(Activity? activity, GetUserAvatarRequest request)
        {
            var counter = _diagnosticProvider.CreateCounter<int>("get.count", "Количество запрашиваемых аватаров пользователей");
            counter?.Add(1);
        }
    }
}
