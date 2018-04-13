using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Repositories {
    public interface IEntityRepository<T> {

        IList<T> All { get; }

        Task<IList<T>> AllAsync { get; }

        IList<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);

        Task<IList<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);

        IList<T> Get(params Expression<Func<T, bool>>[] filters);
        Task<IList<T>> GetAsync(params Expression<Func<T, bool>>[] filters);

        IList<T> GetIncluding(Expression<Func<T, bool>>[] filters, Expression<Func<T, object>>[] includeProperties);
        Task<IList<T>> GetIncludingAsync(Expression<Func<T, bool>>[] filters, Expression<Func<T, object>>[] includeProperties);

        void InsertOrUpdateGraph(T entity);
        void InsertOrUpdate(T entity);

        void Save();
        Task SaveAsync();

        void Delete(T entity);

    }
}
