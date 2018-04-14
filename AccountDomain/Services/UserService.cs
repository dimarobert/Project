using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Project.Account.Managers;
using Project.Account.Models;

namespace Project.Account.Services {

    public interface IUserService {
        bool IsAuthenticated { get; }

        string GetUserId();

        string GetUserName();

        Task<UserInfo> FindUserByName(string userName);
    }

    public class UserService : IUserService {
        readonly ApplicationUserManager userManager;
        readonly IPrincipal user;

        public bool IsAuthenticated => user?.Identity?.IsAuthenticated ?? false;

        public UserService(ApplicationUserManager userManager, IPrincipal user) {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.user = user;
        }


        public string GetUserId() {
            return user?.Identity?.GetUserId();
        }

        public string GetUserName() {
            return user?.Identity?.GetUserName();
        }

        public async Task<UserInfo> FindUserByName(string userName) {
            return await userManager.FindByNameAsync(userName);
        }
    }
}