using Microsoft.AspNet.Identity;
using Project.Account.Models;

namespace Project.Account.Managers {
    public class ApplicationRoleManager : RoleManager<RoleInfo> {
        public ApplicationRoleManager(IRoleStore<RoleInfo, string> store) : base(store) {
        }
    }
}
