using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Factories.Strategies;
using InsuranceGoSmoke.Security.Contracts.Identify.Enums;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Factories
{
    /// <summary>
    /// Фабрика стратегий идентификации.
    /// </summary>
    public interface IIdentifyStrategyFactory
    {
        /// <summary>
        /// Возвращает стратегию идентификации.
        /// </summary>
        /// <param name="identifyType">Тип идентификации.</param>
        /// <returns>Стратегия авторизации.</returns>
        public IIdentifyStrategy GetIdentifyStrategy(IdentifyTypes identifyType);

        /// <summary>
        /// Возвращает стратегию для отправки кода на авторизацию.
        /// </summary>
        /// <param name="identifyType">Тип идентификации.</param>
        /// <returns>Стратегия для отправки кода на авторизацию.</returns>
        public ISendingAuthorizationCodeStrategy GetSendingCodeStrategy(IdentifyTypes identifyType);
    }
}
