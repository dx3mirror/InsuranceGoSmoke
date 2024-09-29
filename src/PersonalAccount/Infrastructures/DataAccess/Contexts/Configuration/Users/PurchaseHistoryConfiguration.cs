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
            // Установка первичного ключа
            entity.HasKey(e => e.ClientId);

            // Установка свойств
            entity.Property(e => e.ClientId)
                .IsRequired();

            entity.Property(e => e.PurchaseDate)
                .IsRequired();

            entity.Property(e => e.StatusPurchased)
                .IsRequired()
                .HasMaxLength(200);

            // Настройка отношений
            entity.HasOne(e => e.User)
                .WithMany(u => u.PurchaseHistories)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
