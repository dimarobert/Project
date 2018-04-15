using Microsoft.AspNet.Identity.EntityFramework;
using Project.Account.Models;
using System.Data.Entity;

namespace Project.Account.Repositories {
    public class ApplicationRoleStore : RoleStore<RoleInfo, string, UserRoleInfo> {
        public ApplicationRoleStore(DbContext context) : base(context) {
        }
    }
}
