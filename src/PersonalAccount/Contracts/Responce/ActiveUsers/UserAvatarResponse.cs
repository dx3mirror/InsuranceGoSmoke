namespace InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers
{
    /// <summary>
    /// Представляет информацию об аватаре пользователя.
    /// </summary>
    public class UserAvatarResponse
    {
        /// <summary>
        /// Получает или устанавливает данные изображения аватара.
        /// </summary>
        public Guid ImageData { get; set; }

        /// <summary>
        /// Получает или устанавливает значение, указывающее, видим ли аватар.
        /// </summary>
        public bool IsVisible { get; set; }
    }

}
