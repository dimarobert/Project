using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Project.Core.DbContext {
    public abstract class BaseDbContext<TContext> : System.Data.Entity.DbContext where TContext : System.Data.Entity.DbContext {

        static BaseDbContext() {
            Database.SetInitializer<TContext>(null);
        }

        public BaseDbContext()
            : base("name=DefaultConnection") {
        }

    }

    
}
