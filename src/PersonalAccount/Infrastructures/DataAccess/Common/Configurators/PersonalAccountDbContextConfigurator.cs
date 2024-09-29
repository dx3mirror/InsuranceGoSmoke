using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurators;
using InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Common.Configurators
{
    /// <summary>
    /// Конфигуратор базы данных примера.
    /// </summary>
    /// <param name="configuration">Конфигурация.</param>
    /// <param name="loggerFactory">Фабрика логгера.</param>
    public class PersonalAccountDbContextConfigurator(IConfiguration configuration, ILoggerFactory loggerFactory)
        : BaseDbContextConfigurator<PersonalAccountsDbContext>(configuration, loggerFactory)
    {
        /// <inheritdoc/>
        protected override string ConnectionStringName => "PersonalAccountDb";
    }
}
