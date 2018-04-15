using Microsoft.AspNet.Identity.EntityFramework;
using Project.Account.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Account.Repositories {
    public class ApplicationUserStore : UserStore<UserInfo, RoleInfo, string, IdentityUserLogin, UserRoleInfo, IdentityUserClaim> {
        public ApplicationUserStore(DbContext context) : base(context) {
        }
    }
}
