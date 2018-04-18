using Project.Core.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Repositories {
    public interface IUnitOfWork : IDisposable {

        void Complete();
        Task CompleteAsync();

    }

    public class UnitOfWork : IUnitOfWork {

        IDbContext context;

        public UnitOfWork(IDbContext context) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Complete() {
            context.SaveChanges();
        }

        public async Task CompleteAsync() {
            await context.SaveChangesAsync();
        }

        public void Dispose() {
            
        }
    }
}
