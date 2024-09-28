using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurations.Converters;

namespace InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurations
{
    /// <summary>
    /// Расширение для сборщика моделей.
    /// </summary>
    public static class ModelBuilderExtension
    {
        /// <summary>
        /// Устанавливает тип дат по-умолчанию.
        /// </summary>
        /// <param name="modelBuilder">Сборщик.</param>
        /// <param name="kind">Тип дат.</param>
        public static void SetDefaultDateTimeKind(this ModelBuilder modelBuilder, DateTimeKind kind)
        {
            modelBuilder.UseValueConverterForType<DateTime>(new DateTimeKindValueConverter(kind));
            modelBuilder.UseValueConverterForType<DateTime?>(new DateTimeKindValueConverter(kind));
        }

        private static ModelBuilder UseValueConverterForType<T>(this ModelBuilder modelBuilder, ValueConverter converter)
        {
            return modelBuilder.UseValueConverterForType(typeof(T), converter);
        }

        internal static ModelBuilder UseValueConverterForType(this ModelBuilder modelBuilder, Type type, ValueConverter converter)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == type);

                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name)
                        .HasConversion(converter);
                }
            }

            return modelBuilder;
        }
    }
}
