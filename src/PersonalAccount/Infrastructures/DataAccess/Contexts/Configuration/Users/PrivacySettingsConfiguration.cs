using InsuranceGoSmoke.PersonalAccount.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts.Configuration.Users
{
    /// <summary>
    /// Конфигурация сущности <see cref="PrivacySettings"/>
    /// </summary>
    public class PrivacySettingsConfiguration : IEntityTypeConfiguration<PrivacySettings>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<PrivacySettings> entity)
        {
            entity.HasKey(e => e.ClientId);
            entity.Property(e => e.ShowEmail).HasDefaultValue(false);
            entity.Property(e => e.ShowBirthdate).HasDefaultValue(false);
            entity.Property(e => e.ShowDescription).HasDefaultValue(false);
        }
    }
}
