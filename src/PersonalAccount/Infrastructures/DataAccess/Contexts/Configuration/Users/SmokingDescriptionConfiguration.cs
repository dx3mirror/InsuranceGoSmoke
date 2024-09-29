using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurators;
using InsuranceGoSmoke.PersonalAccount.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts.Configuration.Users
{
    /// <summary>
    /// Конфигурация сущности <see cref="SmokingDescription"/>
    /// </summary>
    public class SmokingDescriptionConfiguration : IEntityTypeConfiguration<SmokingDescription>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<SmokingDescription> entity)
        {
            entity.HasKey(e => e.ClientId);
            entity.Property(e => e.IsSmoked).HasDefaultValue(true);
            entity.Property(e => e.IsVape).HasDefaultValue(false);
            entity.Property(e => e.IsDrink).HasDefaultValue(false);
            entity.Property(e => e.ReadyMeeting).HasDefaultValue(true);
        }
    }
}
