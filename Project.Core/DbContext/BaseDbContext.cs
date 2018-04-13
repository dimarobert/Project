using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Project.Core.Models;

namespace Project.Core.DbContext {

    public abstract class BaseDbContext<TContext> : System.Data.Entity.DbContext where TContext : System.Data.Entity.DbContext {

        static BaseDbContext() {
            Database.SetInitializer<TContext>(null);
        }

        public BaseDbContext()
            : base("name=DefaultConnection") {
        }

        public virtual void ApplyStateChanges() {

            foreach (var entry in ChangeTracker.Entries<IObjectWithState>()) {
                var stateInfo = entry.Entity;
                entry.State = ModelStateHelpers.ConvertState(stateInfo.State);
                stateInfo.State = ModelState.Unchanged;
            }
        }

    }

    
}
