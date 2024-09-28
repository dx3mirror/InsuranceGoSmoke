namespace InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.Convertor
{
    /// <summary>
    /// Интерфейс для преобразования строк в указанные типы данных.
    /// </summary>
    public interface IStringConverter
    {
        /// <summary>
        /// Универсальный метод для преобразования строки в указанный тип данных.
        /// </summary>
        /// <typeparam name="T">Тип данных, в который нужно преобразовать строку.</typeparam>
        /// <param name="input">Входная строка для преобразования.</param>
        /// <returns>Результат преобразования, содержащий значение типа T или null, если преобразование не удалось.</returns>
        ConversionResult<T> Convert<T>(string input) where T : struct;

        /// <summary>
        /// Универсальный метод для преобразования строки в указанный тип данных с учетом культуры.
        /// </summary>
        /// <typeparam name="T">Тип данных, в который нужно преобразовать строку.</typeparam>
        /// <param name="input">Входная строка для преобразования.</param>
        /// <param name="culture">Культура для преобразования.</param>
        /// <returns>Результат преобразования, содержащий значение типа T или null, если преобразование не удалось.</returns>
        ConversionResult<T> ConvertWithCulture<T>(string input, IFormatProvider culture) where T : struct;
    }

}
