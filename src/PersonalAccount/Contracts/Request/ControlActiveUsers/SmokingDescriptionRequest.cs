namespace InsuranceGoSmoke.PersonalAccount.Contracts.Request.ControlActiveUsers
{
    /// <summary>
    /// Представляет запрос на создание или обновление описания курения пользователя.
    /// </summary>
    /// <remarks>
    /// Этот класс содержит свойства, которые описывают опыт курения пользователя,
    /// его привычки, связанные с курением, вейпингом и употреблением алкоголя. Он используется для 
    /// передачи данных между клиентом и сервером при добавлении или изменении информации о курении пользователя.
    /// </remarks>
    public class SmokingDescriptionRequest
    {
        /// <summary>
        /// Получает или задает идентификатор клиента (пользователя), связанный с описанием курения.
        /// </summary>
        public long ClientId { get; set; }

        /// <summary>
        /// Получает или задает опыт курения в годах. Может быть null, если не указан.
        /// </summary>
        public int? SmokingExperienceYears { get; set; }

        /// <summary>
        /// Получает или задает причину, по которой пользователь начал курить. Может быть null, если не указана.
        /// </summary>
        public string? ReasonStartedSmoking { get; set; }

        /// <summary>
        /// Получает или задает значение, указывающее, является ли пользователь курильщиком. По умолчанию true.
        /// </summary>
        public bool IsSmoked { get; set; } = true;

        /// <summary>
        /// Получает или задает значение, указывающее, использует ли пользователь вейп. По умолчанию false.
        /// </summary>
        public bool IsVape { get; set; } = false;

        /// <summary>
        /// Получает или задает значение, указывающее, употребляет ли пользователь алкоголь. По умолчанию false.
        /// </summary>
        public bool IsDrink { get; set; } = false;

        /// <summary>
        /// Получает или задает значение, указывающее, открыт ли пользователь для встреч. По умолчанию true.
        /// </summary>
        public bool ReadyMeeting { get; set; } = true;

        /// <summary>
        /// Получает или задает дополнительную информацию о пользователе. Может быть null, если не указана.
        /// </summary>
        public string? About { get; set; }
    }
}
