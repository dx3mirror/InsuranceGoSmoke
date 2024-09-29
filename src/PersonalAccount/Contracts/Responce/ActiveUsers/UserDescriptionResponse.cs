namespace InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers
{
    /// <summary>
    /// Представляет описание пользователя.
    /// </summary>
    public class UserDescriptionResponse
    {
        /// <summary>
        /// Получает или задает количество лет опыта курения.
        /// </summary>
        public int? SmokingExperienceYears { get; set; }

        /// <summary>
        /// Получает или задает причину, по которой пользователь начал курить.
        /// </summary>
        public string ReasonStartedSmoking { get; set; }

        /// <summary>
        /// Получает или задает, готов ли пользователь к встрече.
        /// </summary>
        public bool ReadyMeeting { get; set; }

        /// <summary>
        /// Получает или задает, курит ли пользователь.
        /// </summary>
        public bool IsSmoked { get; set; }

        /// <summary>
        /// Получает или задает, использует ли пользователь вейп.
        /// </summary>
        public bool IsVape { get; set; }

        /// <summary>
        /// Получает или задает информацию о пользователе.
        /// </summary>
        public string About { get; set; }
    }

}
