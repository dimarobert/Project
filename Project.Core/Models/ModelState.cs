using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Models {
    public enum ModelState {
        Unchanged = 0,
        Added,
        Modified,
        Deleted
    }

    public static class ModelStateHelpers {
        public static EntityState ConvertState(ModelState state) {
            switch (state) {
                case ModelState.Added:
                    return EntityState.Added;
                case ModelState.Modified:
                    return EntityState.Modified;
                case ModelState.Deleted:
                    return EntityState.Deleted;

                default:
                    return EntityState.Unchanged;
            }
        }
    }
}
