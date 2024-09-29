using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurators;
using InsuranceGoSmoke.PersonalAccount.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts.Configuration.Users
{
    /// <summary>
    /// Конфигурация сущности <see cref="PurchaseHistory"/>
    /// </summary>
    public class PurchaseHistoryConfiguration : IEntityTypeConfiguration<PurchaseHistory>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<PurchaseHistory> entity)
        {
            entity.HasKey(e => new { e.ClientId, e.PurchaseDate });
            entity.Property(e => e.StatusPurchased).IsRequired().HasMaxLength(PropertyLengthConstants.Length200);
        }
    }
}
