using Project.Core.DbContext;
using Project.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Repositories {
    public interface IEntityRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, IObjectWithState
        where TKey : IEquatable<TKey> {

        IList<TEntity> All { get; }

        Task<IList<TEntity>> AllAsync { get; }

        IList<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IList<TEntity>> AllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        IList<TEntity> Get(params Expression<Func<TEntity, bool>>[] filters);
        Task<IList<TEntity>> GetAsync(params Expression<Func<TEntity, bool>>[] filters);

        IList<TEntity> GetIncluding(Expression<Func<TEntity, bool>>[] filters, Expression<Func<TEntity, object>>[] includeProperties);
        Task<IList<TEntity>> GetIncludingAsync(Expression<Func<TEntity, bool>>[] filters, Expression<Func<TEntity, object>>[] includeProperties);

        void InsertOrUpdateGraph(TEntity entity);
        void InsertOrUpdate(TEntity entity);

        void Save();
        Task SaveAsync();

        void Delete(TEntity entity);

    }

    public abstract class EntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, IObjectWithState
        where TKey : IEquatable<TKey> {

        protected readonly IDbContext context;

        public EntityRepository(IDbContext context) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual IList<TEntity> All => GetQ().ToList();

        public virtual Task<IList<TEntity>> AllAsync => Task.Run<IList<TEntity>>(async () => await GetQ().ToListAsync());

        public virtual IList<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties) {
            return GetIncludingQ(includeProperties).ToList();
        }

        public virtual async Task<IList<TEntity>> AllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties) {
            return await GetIncludingQ(includeProperties).ToListAsync();
        }

        public virtual IList<TEntity> Get(params Expression<Func<TEntity, bool>>[] filters) {
            return GetQ(filters).ToList();
        }

        public virtual async Task<IList<TEntity>> GetAsync(params Expression<Func<TEntity, bool>>[] filters) {
            return await GetQ(filters).ToListAsync();
        }

        public virtual IList<TEntity> GetIncluding(Expression<Func<TEntity, bool>>[] filters, Expression<Func<TEntity, object>>[] includeProperties) {
            return GetIncludingQ(GetQ(filters), includeProperties).ToList();
        }

        public virtual async Task<IList<TEntity>> GetIncludingAsync(Expression<Func<TEntity, bool>>[] filters, Expression<Func<TEntity, object>>[] includeProperties) {
            return await GetIncludingQ(GetQ(filters), includeProperties).ToListAsync();
        }

        public virtual void InsertOrUpdate(TEntity entity) {
            if (entity.Id.Equals(default(TKey)))
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void InsertOrUpdateGraph(TEntity entityGraph) {
            context.Set<TEntity>().Add(entityGraph);

            if (entityGraph.State != ModelState.Added)
                context.ApplyStateChanges();
        }

        public virtual void Delete(TEntity entity) {
            context.Set<TEntity>().Remove(entity);
        }

        public virtual void Save() {
            context.SaveChanges();
        }

        public virtual async Task SaveAsync() {
            await context.SaveChangesAsync();
        }

        protected virtual IQueryable<TEntity> GetQ(params Expression<Func<TEntity, bool>>[] filters) {
            return GetQ(context.Set<TEntity>(), filters);
        }

        protected virtual IQueryable<TEntity> GetQ(IQueryable<TEntity> query, params Expression<Func<TEntity, bool>>[] filters) {
            foreach (var filter in filters)
                query = query.Where(filter);

            return query;
        }

        protected virtual IQueryable<TEntity> GetIncludingQ(params Expression<Func<TEntity, object>>[] includeProperties) {
            return GetIncludingQ(context.Set<TEntity>(), includeProperties);
        }

        protected virtual IQueryable<TEntity> GetIncludingQ(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties) {
            foreach (var include in includeProperties)
                query = query.Include(include);

            return query;
        }

    }
}
