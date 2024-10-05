namespace InsuranceGoSmoke.PersonalAccount.Domain
{
    /// <summary>
    /// Класс Avatar содержит информацию об аватарке пользователя,
    /// включая изображение и его видимость, а также историю загрузок.
    /// </summary>
    public class Avatar
    {
        /// <summary>
        /// Уникальный идентификатор клиента, связанный с аватаром (FKGUIDCLIENT).
        /// </summary>
        public long ClientId { get; set; }

        /// <summary>
        /// Данные изображения аватарки.
        /// </summary>
        public required Guid ImageData { get; set; }

        /// <summary>
        /// Указывает, виден ли аватар пользователю и другим.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// История загрузок аватарок, хранит информацию о предыдущих аватарах.
        /// </summary>
        public ICollection<AvatarHistory>? AvatarHistories { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="avatar"></param>
        public Avatar(long clientId, Guid avatar)
        {
            ClientId = clientId;
            ImageData = avatar;
        }
    }
}
