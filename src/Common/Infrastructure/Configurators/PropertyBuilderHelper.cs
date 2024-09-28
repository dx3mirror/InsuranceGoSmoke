using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurators
{
    /// <summary>
    /// Helper для работы с builder'ом свойств.
    /// </summary>
    public static class PropertyBuilderHelper
    {
        /// <summary>
        /// Помечает поле как временную метку.
        /// </summary>
        /// <param name="builder">Builder.</param>
        /// <returns>Builder.</returns>
        public static PropertyBuilder<DateTime> IsTimestamp(this PropertyBuilder<DateTime> builder)
        {
            return builder.HasDefaultValueSql("now()");
        }

        /// <summary>
        /// Помечает поле как Guid.
        /// </summary>
        /// <param name="builder">Builder.</param>
        /// <returns>Builder.</returns>
        public static PropertyBuilder<Guid?> IsGuid(this PropertyBuilder<Guid?> builder)
        {
            return builder.HasColumnType("uuid");
        }

        /// <summary>
        /// Помечает поле как Guid.
        /// </summary>
        /// <param name="builder">Builder.</param>
        /// <returns>Builder.</returns>
        public static PropertyBuilder<Guid> IsGuid(this PropertyBuilder<Guid> builder)
        {
            return builder.HasColumnType("uuid").IsRequired();
        }
    }

    /// <summary>
    /// Константы длины свойств.
    /// </summary>
    public readonly struct PropertyLengthConstants
    {
        /// <summary>
        /// Длина свойства в 3 символа.
        /// </summary>
        public static readonly int Length3 = 3;

        /// <summary>
        /// Длина свойства в 3 символа.
        /// </summary>
        public static readonly int Length20 = 20;

        /// <summary>
        /// Длина свойства в 200 символов.
        /// </summary>
        public static readonly int Length200 = 200;

        /// <summary>
        /// Длина свойства в 2000 символов.
        /// </summary>
        public static readonly int Length2000 = 2000;

        /// <summary>
        /// Длина свойства в 8000 символов.
        /// </summary>
        public static readonly int LengthMax = 8000;

    }
}
