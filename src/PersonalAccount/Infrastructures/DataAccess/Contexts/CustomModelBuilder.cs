using Microsoft.EntityFrameworkCore;
using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurations;
using InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts.Configuration.Users;

namespace InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts
{
    /// <summary>
    /// Сборщик моделей.
    /// </summary>
    public static class CustomModelBuilder
    {
        /// <summary>
        /// Создает модель.
        /// </summary>
        /// <param name="modelBuilder">Билдер.</param>
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureForecastModels(modelBuilder);

            modelBuilder.SetDefaultDateTimeKind(DateTimeKind.Utc);
        }

        private static void ConfigureForecastModels(ModelBuilder modelBuilder)
        {
            // расширение для генерации uuid
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.ApplyConfiguration(new AccountStatusConfiguration());
            modelBuilder.ApplyConfiguration(new AvatarConfiguration());
            modelBuilder.ApplyConfiguration(new AvatarHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new PrivacySettingsConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileDesignConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new SmokingDescriptionConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
        }
    }
}
