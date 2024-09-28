namespace InsuranceGoSmoke.Notification.Contracts.Attributes
{
    /// <summary>
    /// Атрибут названия шаблона.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class TemplateNameAttribute : Attribute
    {
        /// <summary>
        /// Название шаблона.
        /// </summary>
        public string TemplateName { get; }

        /// <summary>
        /// Создаёт экземпляр <see cref="TemplateNameAttribute"/>
        /// </summary>
        /// <param name="templateName">Название шаблона.</param>
        public TemplateNameAttribute(string templateName)
        {
            TemplateName = templateName;
        }
    }
}
