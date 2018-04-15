using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Tests.Utils {
    internal static class DbSetHelpers {

        public static Mock<DbSet<T>> GetDbSetMock<T>(IQueryable<T> items) where T : class {
            var dbSetMock = new Mock<DbSet<T>>();

            var q = dbSetMock.As<IQueryable<T>>();
            q.Setup(s => s.Provider).Returns(new TestDbAsyncQueryProvider<T>(items.Provider));
            q.Setup(s => s.Expression).Returns(items.Expression);
            q.Setup(s => s.ElementType).Returns(items.ElementType);
            q.Setup(s => s.GetEnumerator()).Returns(items.GetEnumerator);

            var qAsync = dbSetMock.As<IDbAsyncEnumerable<T>>();
            qAsync.Setup(s => s.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<T>(items.GetEnumerator()));
            return dbSetMock;
        }

        internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider {
            private readonly IQueryProvider _inner;

            internal TestDbAsyncQueryProvider(IQueryProvider inner) {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression) {
                return new TestDbAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression) {
                return new TestDbAsyncEnumerable<TElement>(expression);
            }

            public object Execute(Expression expression) {
                return _inner.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression) {
                return _inner.Execute<TResult>(expression);
            }

            public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken) {
                return Task.FromResult(Execute(expression));
            }

            public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) {
                return Task.FromResult(Execute<TResult>(expression));
            }
        }

        internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T> {
            public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
                : base(enumerable) { }

            public TestDbAsyncEnumerable(Expression expression)
                : base(expression) { }

            public IDbAsyncEnumerator<T> GetAsyncEnumerator() {
                return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }

            IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator() {
                return GetAsyncEnumerator();
            }

            IQueryProvider IQueryable.Provider {
                get { return new TestDbAsyncQueryProvider<T>(this); }
            }
        }

        internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T> {
            private readonly IEnumerator<T> enumerator;

            public T Current => enumerator.Current;

            object IDbAsyncEnumerator.Current => Current;

            public TestDbAsyncEnumerator(IEnumerator<T> enumerator) {
                this.enumerator = enumerator;
            }

            public void Dispose() {
                enumerator?.Dispose();
            }

            public Task<bool> MoveNextAsync(CancellationToken cancellationToken) {
                return Task.FromResult(enumerator.MoveNext());
            }
        }

    }
}
