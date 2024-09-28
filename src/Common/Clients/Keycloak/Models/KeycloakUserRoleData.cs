namespace InsuranceGoSmoke.Common.Clients.Keycloak.Models
{
    /// <summary>
    /// Данные роли.
    /// </summary>
    public class KeycloakUserRoleData
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="KeycloakUserRoleData"/>
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        public KeycloakUserRoleData(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
