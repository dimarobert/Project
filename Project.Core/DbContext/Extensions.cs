using Project.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.DbContext {
    public static class Extensions {

        public static void ApplyStateChanges(this IDbContext context) {

            foreach(var entry in context.ChangeTracker.Entries<IObjectWithState>()) {
                var stateInfo = entry.Entity;
                entry.State = ModelStateHelpers.ConvertState(stateInfo.State);
                stateInfo.State = ModelState.Unchanged;
            }
        }
    }
}
