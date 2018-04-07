using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Project.Account.Services {

    public interface IUserService {
        bool IsAuthenticated { get; }

        string GetUserId();
    }

    public class UserService : IUserService {
        readonly IPrincipal user;

        public bool IsAuthenticated => user?.Identity?.IsAuthenticated ?? false;

        public UserService(IPrincipal user) {
            this.user = user;
        }


        public string GetUserId() {
            return user?.Identity?.GetUserId();
        }
    }
}