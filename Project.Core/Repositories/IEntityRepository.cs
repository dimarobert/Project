using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Repositories {
    public interface IEntityRepository<T> {

        IList<T> All { get; }

        IList<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);

        IList<T> Get(params Expression<Func<T, object>>[] filters);
        IList<T> GetIncluding(Expression<Func<T, object>>[] filters, Expression<Func<T, object>>[] includeProperties);

        void InsertOrUpdate(T entity);
        void Save();

        void Delete(T entity);

    }
}
