using InsuranceGoSmoke.PersonalAccount.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts.Configuration.Users
{
    /// <summary>
    /// Конфигурация сущности <see cref="Avatar"/>
    /// </summary>
    public class AvatarConfiguration : IEntityTypeConfiguration<Avatar>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Avatar> entity) 
        {
            entity.HasKey(e => e.ClientId);
            entity.HasMany(e => e.AvatarHistories)
                  .WithOne()
                  .HasForeignKey(ah => ah.ClientId);
        }
    }
}
