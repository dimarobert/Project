using Moq;
using Project.Core.DbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Tests.Utils {
    public static class MockingHelpers {

        public static void MockDbContextSet<TContext, TEntity>(Mock<TContext> mock, Expression<Func<TContext, DbSet<TEntity>>> setSelection, IQueryable<TEntity> values)
            where TContext : class, IDbContext
            where TEntity : class {
            var dbSet = values != null ? DbSetHelpers.GetDbSetMock(values) : null;

            mock.Setup(setSelection).Returns(dbSet?.Object);
        }
    }
}
