using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Tests.Utils {
    internal static class DbSetHelpers {

        public static Mock<DbSet<T>> GetDbSetMock<T>(IQueryable<T> items) where T : class {
            var taskData = new Mock<DbSet<T>>();
            var q = taskData.As<IQueryable<T>>();
            q.Setup(t => t.Provider).Returns(items.Provider);
            q.Setup(t => t.Expression).Returns(items.Expression);
            q.Setup(t => t.ElementType).Returns(items.ElementType);
            q.Setup(t => t.GetEnumerator()).Returns(items.GetEnumerator);
            return taskData;
        }

    }
}
