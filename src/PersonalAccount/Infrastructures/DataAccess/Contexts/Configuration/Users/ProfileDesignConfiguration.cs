using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurators;
using InsuranceGoSmoke.PersonalAccount.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts.Configuration.Users
{
    /// <summary>
    /// Конфигурация сущности <see cref="ProfileDesign"/>
    /// </summary>
    public class ProfileDesignConfiguration : IEntityTypeConfiguration<ProfileDesign>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<ProfileDesign> entity)
        {
            entity.HasKey(e => e.ClientId);
            entity.Property(e => e.ThemeColor).HasMaxLength(PropertyLengthConstants.Length200);
            entity.Property(e => e.BackgroundImage).HasMaxLength(PropertyLengthConstants.Length200);
            entity.Property(e => e.FontStyle).HasMaxLength(PropertyLengthConstants.Length200);
            entity.Property(e => e.EnableAnimations).HasDefaultValue(true);
        }
    }
}
