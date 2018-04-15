using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Account.Models {
    public class RoleInfo : IdentityRole<string, UserRoleInfo> {

        public RoleInfo() {
            Id = Guid.NewGuid().ToString();
        }

    }

    public class UserRoleInfo : IdentityUserRole<string> {

    }
}
