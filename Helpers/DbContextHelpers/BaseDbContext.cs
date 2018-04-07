using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Helpers.DbContextHelpers {
    public class BaseDbContext<TContext> : DbContext where TContext : DbContext {


        static BaseDbContext() {
            Database.SetInitializer<TContext>(null);
        }

        public BaseDbContext()
            : base("name=DefaultConnection") {
        }

    }
}
