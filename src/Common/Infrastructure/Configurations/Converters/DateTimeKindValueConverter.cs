using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurations.Converters
{
    /// <summary>
    /// Конвертер значений типов дат.
    /// </summary>
    /// <param name="kind">Тип дат.</param>
    /// <param name="mappingHints">Подсказки маппинга.</param>
    public class DateTimeKindValueConverter(DateTimeKind kind, ConverterMappingHints? mappingHints = null) 
        : ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, kind),
            mappingHints)
    {
    }
}
