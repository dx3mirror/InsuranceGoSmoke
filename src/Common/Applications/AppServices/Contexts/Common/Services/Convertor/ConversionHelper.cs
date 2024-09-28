using System.Globalization;

namespace InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.Convertor
{
    /// <summary>
    /// Содержит вспомогательные методы для преобразования строк в различные типы данных.
    /// </summary>
    public static class ConversionHelper
    {
        /// <summary>
        /// Преобразует строку в указанный тип данных и возвращает значение по умолчанию, если преобразование не удалось.
        /// </summary>
        /// <typeparam name="T">Тип данных для преобразования.</typeparam>
        /// <param name="input">Входная строка для преобразования.</param>
        /// <param name="defaultValue">Значение по умолчанию, которое будет возвращено, если преобразование не удалось.</param>
        /// <param name="converter">Конвертер для выполнения преобразования.</param>
        /// <param name="useCulture">Флаг для использования культуры при преобразовании.</param>
        /// <param name="culture">Культура для преобразования (если useCulture = true).</param>
        /// <returns>Значение типа T или значение по умолчанию.</returns>
        public static T ConvertOrDefault<T>(string input, T defaultValue, IStringConverter converter, bool useCulture = false, IFormatProvider? culture = null) where T : struct
        {
            var result = useCulture
                ? converter.ConvertWithCulture<T>(input, culture ?? CultureInfo.InvariantCulture)
                : converter.Convert<T>(input);

            return result.GetValueOrDefault(defaultValue);
        }
    }
}
