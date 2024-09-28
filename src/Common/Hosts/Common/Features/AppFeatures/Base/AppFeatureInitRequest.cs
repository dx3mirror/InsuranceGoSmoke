using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base
{
    /// <summary>
    /// Запрос на инициализацию функциональности.
    /// </summary>
    public class AppFeatureInitRequest
    {
        /// <summary>
        /// Наименование.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Порядок применения.
        /// </summary>
        public required int Order { get; set; }

        /// <summary>
        /// Глобальные настройки.
        /// </summary>
        public required IConfiguration Configuration { get; set; }

        /// <summary>
        /// Секция настроек.
        /// </summary>
        public required IConfigurationSection OptionSection { get; set; }

        /// <summary>
        /// Дополнительные сборки.
        /// </summary>
        public required Assembly[] AdditionalAssemblies { get; set; } = [];
    }
}
