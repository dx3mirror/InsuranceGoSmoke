using Microsoft.EntityFrameworkCore;

namespace InsuranceGoSmoke.Common.Infrastructures.DataAccess.Session
{
    /// <summary>
    /// Фабрика сессии данных.
    /// </summary>
    /// <param name="dbContext">Контекст.</param>
    public class EntityFrameworkDataSessionFactory(DbContext dbContext) : IDataSessionFactory
    {
        /// <inheritdoc />
        public IDataSession Create()
        {
            return new EntityFrameworkDataSession(dbContext);
        }
    }
}
