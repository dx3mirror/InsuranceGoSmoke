using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InsuranceGoSmoke.PersonalAccount.Domain;

namespace InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts.Configuration.Users
{
    /// <summary>
    /// Конфигурация сущности <see cref="AccountStatus"/>
    /// </summary>
    public class AccountStatusConfiguration : IEntityTypeConfiguration<AccountStatus>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<AccountStatus> entity)
        {
            entity.HasKey(e => e.ClientId);
            entity.Property(e => e.IsVisible).HasDefaultValue(true);
            entity.Property(e => e.IsAccessible).HasDefaultValue(true);
            entity.Property(e => e.IsBlocked).HasDefaultValue(false);
            entity.Property(e => e.IsPremium).HasDefaultValue(false);
        }
    }
}
