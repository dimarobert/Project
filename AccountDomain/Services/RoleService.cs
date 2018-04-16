using Project.Account.DAL;
using Project.Account.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Account.Services {
    public interface IRoleService {

        Task<IList<UserInfo>> GetUsersInRoleAsync(string roleName);

    }

    public class RoleService : IRoleService {

        readonly IAccountDbContext context;

        public RoleService(IAccountDbContext context) {
            this.context = context;
        }

        public async Task<IList<UserInfo>> GetUsersInRoleAsync(string roleName) {
            return await context.Roles.Where(r => r.Name == roleName).SelectMany(r => r.Users).Select(ur => ur.User).ToListAsync();
        }
    }
}
