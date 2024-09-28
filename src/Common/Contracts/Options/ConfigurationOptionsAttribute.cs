namespace InsuranceGoSmoke.Common.Contracts.Options
{
    /// <summary>
    /// Атрибут именованных настроек.
    /// Свойство <see cref="SectionName"/> должно указывать на существующую секцию настроек в appsettings.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ConfigurationOptionsAttribute : Attribute
    {
        /// <summary>
        /// Наименование секции в конфигурации, содержащее параметры настроек.
        /// </summary>
        public string SectionName { get; }

        /// <summary>
        /// Создает экземпляр <see cref="ConfigurationOptionsAttribute"/>.
        /// </summary>
        /// <param name="sectionName">Наименование секции настроек.</param>
        public ConfigurationOptionsAttribute(string sectionName)
        {
            SectionName = sectionName;
        }
    }
}
