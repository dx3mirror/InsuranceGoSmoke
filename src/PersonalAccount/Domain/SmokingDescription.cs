namespace InsuranceGoSmoke.PersonalAccount.Domain
{
    /// <summary>
    /// Класс SmokingDescription содержит описание курильщика, включая опыт курения, 
    /// причины начала курения и другие привычки, связанные с курением, вейпингом и алкоголем.
    /// </summary>
    public class SmokingDescription
    {
        /// <summary>
        /// Уникальный идентификатор клиента, связанный с его описанием ().
        /// </summary>
        public required Int32 ClientId { get; set; }

        /// <summary>
        /// Опыт курения в годах (nullable).
        /// </summary>
        public Int32? SmokingExperienceYears { get; set; }

        /// <summary>
        /// Причина, по которой пользователь начал курить (nullable).
        /// </summary>
        public String? ReasonStartedSmoking { get; set; }

        /// <summary>
        /// Указывает, является ли пользователь курильщиком. По умолчанию true.
        /// </summary>
        public bool IsSmoked { get; set; } = true;

        /// <summary>
        /// Указывает, использует ли пользователь вейп. По умолчанию false.
        /// </summary>
        public bool IsVape { get; set; } = false;

        /// <summary>
        /// Указывает, употребляет ли пользователь алкоголь. По умолчанию false.
        /// </summary>
        public bool IsDrink { get; set; } = false;

        /// <summary>
        /// Указывает, открыт ли пользователь для встреч. По умолчанию true.
        /// </summary>
        public bool ReadyMeeting { get; set; } = true;

        /// <summary>
        /// Дополнительная информация о пользователе.
        /// </summary>
        public String? About { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientId"></param>
        public SmokingDescription(Int32 clientId)
        {
            ClientId = clientId;
        }
    }

}
