using System.ComponentModel.DataAnnotations;

namespace InsuranceGoSmoke.Common.Domain
{
    /// <summary>
    /// Сущность с версионностью.
    /// </summary>
    /// <typeparam name="TKey">Тип первичного ключа.</typeparam>
    public abstract class EntityRowVersion<TKey> : Entity<TKey> 
        where TKey : struct
    {
        /// <summary>
        /// Версия .
        /// </summary>
        [ConcurrencyCheck]
        public virtual byte[] RowVersion { get; set; } = [];
    }
}
