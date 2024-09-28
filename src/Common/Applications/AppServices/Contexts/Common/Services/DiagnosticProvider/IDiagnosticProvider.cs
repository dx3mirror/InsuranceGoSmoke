using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DiagnosticProvider
{
    /// <summary>
    /// Провайдер для работы с диагностикой.
    /// </summary>
    public interface IDiagnosticProvider
    {
        /// <summary>
        /// Запускает активность.
        /// </summary>
        /// <param name="assembly">Сборка.</param>
        /// <returns></returns>
        Activity? StartActivity(Assembly? assembly);

        /// <summary>
        /// Создает счетчик.
        /// </summary>
        /// <typeparam name="T">Тип счетчика.</typeparam>
        /// <param name="name">Наименовение.</param>
        /// <returns>Счетчик.</returns>
        Counter<T>? CreateCounter<T>(string name) where T : struct;

        /// <summary>
        /// Создает счетчик.
        /// </summary>
        /// <typeparam name="T">Тип счетчика.</typeparam>
        /// <param name="name">Наименовение.</param>
        /// <param name="description">Описание.</param>
        /// <returns>Счетчик.</returns>
        Counter<T>? CreateCounter<T>(string name, string? description) where T : struct;
    }
}
