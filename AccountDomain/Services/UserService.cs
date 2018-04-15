using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Project.Account.Managers;
using Project.Account.Models;
using Project.Core.Account;

namespace Project.Account.Services {

    public interface IUserService {
        bool IsAuthenticated { get; }

        string GetUserId();

        string GetUserName();

        UserInfo FindUserByName(string userName);
        Task<UserInfo> FindUserByNameAsync(string userName);
        bool IsInRole(StandardRoles role);
        bool IsInRole(string role);
    }

    public class UserService : IUserService {
        readonly ApplicationUserManager userManager;
        readonly IPrincipal user;

        public bool IsAuthenticated => user?.Identity?.IsAuthenticated ?? false;

        public UserService(ApplicationUserManager userManager, IPrincipal user) {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.user = user;
        }

        public bool IsInRole(StandardRoles role) => IsInRole(role.ToString());

        public bool IsInRole(string role) {
            return user?.IsInRole(role) ?? false;
        }

        public string GetUserId() {
            return user?.Identity?.GetUserId();
        }

        public string GetUserName() {
            return user?.Identity?.GetUserName();
        }

        public UserInfo FindUserByName(string userName) {
            return userManager.FindByName(userName);
        }

        public async Task<UserInfo> FindUserByNameAsync(string userName) {
            return await userManager.FindByNameAsync(userName);
        }
    }
}