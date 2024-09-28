using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Factories.Strategies;
using InsuranceGoSmoke.Security.Contracts.Identify.Enums;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Factories
{
    /// <inheritdoc/>
    public class IdentifyStrategyFactory : IIdentifyStrategyFactory
    {
        private readonly SmsIdentifyStrategy _smsIdentifyStrategy;

        /// <summary>
        /// Создаёт экземпляр <see cref="IdentifyStrategyFactory"/>
        /// </summary>
        /// <param name="smsIdentifyStrategy"></param>
        public IdentifyStrategyFactory(SmsIdentifyStrategy smsIdentifyStrategy)
        {
            _smsIdentifyStrategy = smsIdentifyStrategy;
        }

        /// <inheritdoc/>
        public IIdentifyStrategy GetIdentifyStrategy(IdentifyTypes identifyType)
        {
            return identifyType switch
            {
                IdentifyTypes.Sms => _smsIdentifyStrategy,
                _ => throw new ArgumentOutOfRangeException("Необрабатываемая стратегия идентификации: " + identifyType),
            };
        }

        /// <inheritdoc/>
        public ISendingAuthorizationCodeStrategy GetSendingCodeStrategy(IdentifyTypes identifyType)
        {
            return identifyType switch
            {
                IdentifyTypes.Sms => _smsIdentifyStrategy,
                _ => throw new ArgumentOutOfRangeException("Необрабатываемая стратегия отправки кода: " + identifyType),
            };
        }
    }
}