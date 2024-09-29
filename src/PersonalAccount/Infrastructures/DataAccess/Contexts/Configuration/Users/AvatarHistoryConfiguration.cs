using InsuranceGoSmoke.PersonalAccount.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts.Configuration.Users
{
    /// <summary>
    /// Конфигурация сущности <see cref="AvatarHistory"/>
    /// </summary>
    public class AvatarHistoryConfiguration : IEntityTypeConfiguration<AvatarHistory>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<AvatarHistory> entity)
        {
            entity.HasKey(e => new { e.ClientId, e.UploadDate });
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        }
    }
}
