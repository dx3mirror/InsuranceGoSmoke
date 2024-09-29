using InsuranceGoSmoke.PersonalAccount.Domain.Account;

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
        public long ClientId { get; set; }

        /// <summary>
        /// Дата и время, когда была осуществлена покупка.
        /// </summary>
        public required DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Статус приобретённого товара (например, премиум-статус).
        /// </summary>
        public required String StatusPurchased { get; set; }

        /// <summary>
        /// Cсылка на пользолвателя
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Ctor EF
        /// </summary>
        public PurchaseHistory()
        {
            
        }
        /// <summary>
        /// Конструктор класса PurchaseHistory, принимающий уникальный идентификатор клиента.
        /// </summary>
        /// <param name="clientId">Уникальный идентификатор клиента.</param>
        public PurchaseHistory(long clientId)
        {
            ClientId = clientId;
        }
    }
}
