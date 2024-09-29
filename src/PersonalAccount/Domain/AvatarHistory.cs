namespace InsuranceGoSmoke.PersonalAccount.Domain
{
    /// <summary>
    /// Класс AvatarHistory хранит информацию о предыдущих загрузках аватарок.
    /// </summary>
    public class AvatarHistory
    {
        /// <summary>
        /// Уникальный идентификатор клиента, связанный с историей загрузки (FKGUIDCLIENT).
        /// </summary>
        public required long ClientId { get; set; }

        /// <summary>
        /// Данные изображения аватарки в байтах.
        /// </summary>
        public required Guid ImageData { get; set; }

        /// <summary>
        /// Дата и время загрузки аватарки.
        /// </summary>
        public required DateTime UploadDate { get; set; }

        /// <summary>
        /// Указывает, была ли аватарка активной (включенной).
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// CtorEF
        /// </summary>
        public AvatarHistory()
        {
            
        }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="avatar"></param>
        public AvatarHistory(long clientId, Guid avatar)
        {
            ClientId = clientId;
            ImageData = avatar;
        }
    }
}
