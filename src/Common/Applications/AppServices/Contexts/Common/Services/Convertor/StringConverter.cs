using System.Globalization;

namespace InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.Convertor
{
    /// <summary>
    /// Класс, реализующий интерфейс <see cref="IStringConverter"/> для преобразования строк в указанные типы данных.
    /// </summary>
    public class StringConverter : IStringConverter
    {
        /// <summary>
        /// Универсальный метод для преобразования строки в указанный тип данных.
        /// </summary>
        /// <typeparam name="T">Тип данных, в который нужно преобразовать строку.</typeparam>
        /// <param name="input">Входная строка для преобразования.</param>
        /// <returns>Значение типа T или null, если преобразование не удалось.</returns>
        public ConversionResult<T> Convert<T>(string? input) where T : struct
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new ConversionResult<T>(default, false);
            }

            var targetType = typeof(T);
            var tryParseMethod = targetType.GetMethod("TryParse", [typeof(string), targetType.MakeByRefType()]);

            if (tryParseMethod != null)
            {
                var instance = Activator.CreateInstance(targetType) ?? default(T);
                var parameters = new object[] { input, instance };
                var result = tryParseMethod.Invoke(null, parameters);

                if (result is bool success && success)
                {
                    return new ConversionResult<T>((T)parameters[1], true);
                }
            }

            return new ConversionResult<T>(default, false);
        }

        /// <summary>
        /// Универсальный метод для преобразования строки в указанный тип данных с учетом культуры.
        /// </summary>
        /// <typeparam name="T">Тип данных, в который нужно преобразовать строку.</typeparam>
        /// <param name="input">Входная строка для преобразования.</param>
        /// <param name="culture">Культура для конвертации.</param>
        /// <returns>Значение типа T или null, если преобразование не удалось.</returns>
        public ConversionResult<T> ConvertWithCulture<T>(string? input, IFormatProvider culture) where T : struct
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new ConversionResult<T>(default, false);
            }

            var targetType = typeof(T);

            if (targetType == typeof(decimal) && decimal.TryParse(input, NumberStyles.Any, culture, out decimal decimalResult))
            {
                return new ConversionResult<T>((T)(object)decimalResult, true);
            }

            var tryParseMethod = targetType.GetMethod("TryParse", [typeof(string), targetType.MakeByRefType(), typeof(IFormatProvider)]);

            if (tryParseMethod != null)
            {
                var instance = Activator.CreateInstance(targetType, culture) ?? default(T);
                var parameters = new object[] { input, instance };
                var result = tryParseMethod.Invoke(null, parameters);

                if (result is bool success && success)
                {
                    return new ConversionResult<T>((T)parameters[1], true);
                }
            }

            return new ConversionResult<T>(default, false);
        }
    }
}
