using Microsoft.Extensions.DependencyInjection;

namespace InsuranceGoSmoke.Common.Contracts.Contracts.DI
{
    /// <summary>
    /// Ленивый instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyInstance<T> : Lazy<T> where T : notnull
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="serviceProvider">Провайдер сервисов.</param>
        public LazyInstance(IServiceProvider serviceProvider)
            : base(() => serviceProvider.GetRequiredService<T>())
        {
        }
    }
}
