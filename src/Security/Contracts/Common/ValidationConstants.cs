namespace InsuranceGoSmoke.Security.Contracts.Common
{
    /// <summary>
    /// Константы валидации.
    /// </summary>
    public static class ValidationConstants
    {
        /// <summary>
        /// Шаблон валидного номера телефона.
        /// </summary>
        /// <remarks>Взят с smsc.ru, при этом запретил номера на 8, чтобы не было дублей номеров с +7 и 8</remarks>
        public static readonly string ValidPhoneNumberPattern =
            @"^((\+?7)(?!95[4-7]|99[08]|907|94[^09]|336)([348]\d|9[0-6789]|7[0247])\d{8}|\+?(99[^4568]\d{7,11}|994\d{9}|995[57]\d{8}|996[57]\d{8}|998[3789]\d{8}|380[34569]\d{8}|375[234]\d{8}|372\d{7,8}|37[0-4]\d{8}))$";

        /// <summary>
        /// Шаблон валидного цифрового кода.
        /// </summary>
        public static readonly string ValidCodePattern = @"^(\d{4})$";

        /// <summary>
        /// Шаблон валидного Email.
        /// </summary>
        public static readonly string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    }
}
