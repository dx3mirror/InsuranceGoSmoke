using InsuranceGoSmoke.Common.Domain;
using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurators;
using InsuranceGoSmoke.PersonalAccount.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts.Configuration.Users
{
    /// <summary>
    /// Конфигурация сущности <see cref="User"/>
    /// </summary>
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(t => t.ClientGuid).IsGuid().IsRequired();
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(PropertyLengthConstants.Length200);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(PropertyLengthConstants.Length200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(PropertyLengthConstants.Length200);
            entity.Property(e => e.DateOfBirth).IsRequired();

            entity.HasOne(e => e.AccountStatus)
                  .WithOne()
                  .HasForeignKey<User>(e => e.Id);

            entity.HasOne(e => e.PrivacySettings)
                  .WithOne()
                  .HasForeignKey<User>(e => e.Id);

            entity.HasOne(e => e.ProfileDesign)
                  .WithOne()
                  .HasForeignKey<User>(e => e.Id);

            entity.HasOne(e => e.Avatar)
                  .WithOne()
                  .HasForeignKey<User>(e => e.Id);

            entity.HasOne(e => e.SmokingDescription)
                  .WithOne()
                  .HasForeignKey<User>(e => e.Id);

            entity.HasMany(u => u.PurchaseHistories)
            .WithOne(ph => ph.User)
            .HasForeignKey(ph => ph.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
