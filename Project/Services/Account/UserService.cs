using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Project.Services.Account {

    public interface IUserService {
        bool IsAuthenticated { get; }

        string GetUserId();
    }

    public class UserService : IUserService {
        readonly Func<IPrincipal> getUser;

        public bool IsAuthenticated => getUser()?.Identity?.IsAuthenticated ?? false;

        public UserService(Func<IPrincipal> getUser) {
            this.getUser = getUser;
        }


        public string GetUserId() {
            return getUser()?.Identity?.GetUserId();
        }
    }
}