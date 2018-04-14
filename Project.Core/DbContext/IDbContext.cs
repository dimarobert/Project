using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Project.Core.DbContext {
    public interface IDbContext {
        int SaveChanges();
        Task<int> SaveChangesAsync();

        DbEntityEntry Entry(object entity);

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbChangeTracker ChangeTracker { get; }
    }
}
