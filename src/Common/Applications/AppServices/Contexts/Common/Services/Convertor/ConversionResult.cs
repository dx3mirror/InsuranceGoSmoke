namespace InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.Convertor
{
    /// <summary>
    /// Представляет результат операции преобразования, указывая на успешность и предоставляя преобразованное значение.
    /// </summary>
    /// <typeparam name="T">Тип преобразованного значения.</typeparam>
    public class ConversionResult<T>
    {
        /// <summary>
        /// Получает значение, указывающее на то, была ли операция преобразования успешной.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Получает значение, полученное в результате преобразования, или <c>null</c>, если преобразование не удалось.
        /// </summary>
        public T? Value { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ConversionResult{T}"/>.
        /// </summary>
        /// <param name="value">Значение, полученное в результате преобразования. Может быть <c>null</c>.</param>
        /// <param name="success">Значение, указывающее на успешность преобразования.</param>
        public ConversionResult(T? value, bool success)
        {
            Success = success;
            Value = value;
        }

        /// <summary>
        /// Возвращает значение, если преобразование было успешным и значение не является <c>null</c>;
        /// в противном случае возвращает значение по умолчанию.
        /// </summary>
        /// <param name="defaultValue">Значение по умолчанию, которое будет возвращено, если преобразование не удалось.</param>
        /// <returns>Преобразованное значение, если преобразование успешно; иначе значение по умолчанию.</returns>
        public T GetValueOrDefault(T defaultValue)
        {
            if (Success && Value != null)
            {
                return Value;
            }

            return defaultValue;
        }
    }

}
