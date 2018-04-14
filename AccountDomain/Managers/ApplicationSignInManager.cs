using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Project.Account.Models;

namespace Project.Account.Managers {
    public class ApplicationSignInManager : SignInManager<UserInfo, string> {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(UserInfo user) {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }
    }
}
