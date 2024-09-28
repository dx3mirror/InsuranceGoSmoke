namespace InsuranceGoSmoke.Common.Contracts.Utilities.Helpers
{
    /// <summary>
    /// Helper для работы со строками.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Преобразует строку в булевое значение.
        /// </summary>
        /// <param name="string">Строка.</param>
        /// <param name="defaultValue">Значение по-умолчанию.</param>
        /// <returns>Булевое значение.</returns>
        public static bool ToBool(this string? @string, bool defaultValue = false)
        {
            var result = @string.ToNullBool();
            return result ?? defaultValue;
        }

        /// <summary>
        /// Преобразует строку в булевое значение.
        /// </summary>
        /// <param name="string">Строка.</param>
        /// <returns>Булевое значение.</returns>
        public static bool? ToNullBool(this string? @string)
        {
            if (bool.TryParse(@string, out var result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Делает первый символ строки в верхнем регистре.
        /// </summary>
        /// <param name="string">Строка.</param>
        /// <returns>Строка, первый символ которой в верхнем регистре.</returns>
        public static string? FirstCharToUpper(this string? @string)
        {
            if (string.IsNullOrEmpty(@string))
            {
                return null;
            }
            return $"{char.ToUpper(@string[0])}{@string[1..]}";
        }

        /// <summary>
        /// Преобразует строку в число.
        /// </summary>
        /// <param name="string">Строка.</param>
        /// <returns>Число.</returns>
        public static int? ToInt(this string? @string)
        {
            if (string.IsNullOrEmpty(@string))
            {
                return null;
            }

            if(!int.TryParse(@string, out int result))
            {
                return null;
            }

            return result;
        }
    }
}
