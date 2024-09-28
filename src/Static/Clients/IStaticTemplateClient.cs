namespace InsuranceGoSmoke.Static.Clients
{
    /// <summary>
    /// Клиент работы с шаблонами.
    /// </summary>
    public interface IStaticTemplateClient
    {
        /// <summary>
        /// Скачивает шаблон.
        /// </summary>
        /// <param name="templateName">Имя шаблона.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Содержимое шаблона.</returns>
        Task<string> DownloadTemplateAsync(string templateName, CancellationToken cancellationToken);
    }
}
