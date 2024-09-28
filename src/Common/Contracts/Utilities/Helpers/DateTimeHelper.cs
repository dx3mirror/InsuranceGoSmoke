using System.Globalization;

namespace InsuranceGoSmoke.Common.Contracts.Utilities.Helpers
{
    /// <summary>
    /// Helper для работы с датой и временем.
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Преобразует строку в дату.
        /// </summary>
        /// <param name="string">Строка.</param>
        /// <returns>Дата.</returns>
        public static DateTime? ToDateTime(this string? @string)
        {
            var isSuccess = DateTime.TryParse(@string, CultureInfo.CurrentCulture, out DateTime date);
            if (isSuccess)
            {
                return date;
            }

            return null;
        }

        /// <summary>
        /// Преобразует дату в строку.
        /// </summary>
        /// <param name="date">Дата.</param>
        /// <returns>Строка.</returns>
        public static string? ToDateString(this DateTime? date)
        {
            if (date is null)
            {
                return null;
            }

            return date.Value.ToString("dd.MM.yyyy HH:mm");
        }

        /// <summary>
        /// Задаёт тип даты.
        /// </summary>
        /// <param name="dateTime">Дата.</param>
        /// <param name="kind">Тип.</param>
        /// <returns>Дата с заданным типом.</returns>
        public static DateTime? SpecifyKind(this DateTime? dateTime, DateTimeKind kind)
        {
            if (dateTime is null)
            {
                return null;
            }
            return DateTime.SpecifyKind(dateTime.Value, kind);
        }
    }
}
