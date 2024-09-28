namespace InsuranceGoSmoke.PersonalAccount.Domain
{
    /// <summary>
    /// Класс PurchaseHistory содержит информацию о покупках клиента,
    /// включая дату покупки и статус приобретённого товара.
    /// </summary>
    public class PurchaseHistory
    {
        /// <summary>
        /// Уникальный идентификатор клиента, связанный с историей покупок (FKGUIDCLIENT).
        /// </summary>
        public Int32 ClientId { get; set; }

        /// <summary>
        /// Дата и время, когда была осуществлена покупка.
        /// </summary>
        public required DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Статус приобретённого товара (например, премиум-статус).
        /// </summary>
        public required String StatusPurchased { get; set; }

        /// <summary>
        /// Конструктор класса PurchaseHistory, принимающий уникальный идентификатор клиента.
        /// </summary>
        /// <param name="clientId">Уникальный идентификатор клиента.</param>
        public PurchaseHistory(Int32 clientId)
        {
            ClientId = clientId;
        }
    }
}
