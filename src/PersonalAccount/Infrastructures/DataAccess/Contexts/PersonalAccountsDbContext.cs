using Microsoft.EntityFrameworkCore;

namespace InsuranceGoSmoke.PersonalAccount.Infrastructures.DataAccess.Contexts
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    /// <param name="options">Настройки.</param>
    public class PersonalAccountsDbContext(DbContextOptions<PersonalAccountsDbContext> options) : DbContext(options)
    {
        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            CustomModelBuilder.OnModelCreating(modelBuilder);
        }
    }
}
