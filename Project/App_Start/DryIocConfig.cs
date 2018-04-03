using DryIoc;
using DryIoc.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Project.DAL.Tasks;
using Project.Managers.Tasks;
using Project.Models.Account;
using Project.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Project {
    public class DryIocConfig {

        private static Lazy<IContainer> container = new Lazy<IContainer>(() => {
            var container = new Container();
            RegisterTypes(container);
            return container;
        });

        internal static void WithMvc() {
            var mvcContainer = GetContainer().WithMvc(throwIfUnresolved: type => type.IsController());
            container = new Lazy<IContainer>(() => mvcContainer);
        }

        public static IContainer GetContainer() {
            return container.Value;
        }

        public static void RegisterTypes(IContainer container) {
            // Register Identity types
            container.Register<AccountDbContext>(Reuse.InWebRequest);
            container.Register<ApplicationSignInManager>(Reuse.InWebRequest);
            container.Register<ApplicationUserManager>(Reuse.InWebRequest);

            container.Register<IAuthenticationManager>(Reuse.InWebRequest, Made.Of(() => AuthenticationManagerFactory()));
            container.Register<IUserStore<UserInfo>>(Reuse.InWebRequest, Made.Of(() => new UserStore<UserInfo>(Arg.Of<AccountDbContext>())));

            // Register application types
            container.Register<IPrincipal>(Reuse.InWebRequest, Made.Of(() => PrincipalFactory()));
            container.Register<IUserService, UserService>(Reuse.InWebRequest);
            container.Register<ITaskManager, TaskManager>(Reuse.InWebRequest);

            container.Register<ITaskDbContext, TaskDbContext>(Reuse.InWebRequest, Made.Of(() => new TaskDbContext()));

        }

        internal static void RegisterOwinTypes(IAppBuilder app) {
            var container = GetContainer();

            container.RegisterInstance<IDataProtectionProvider>(app.GetDataProtectionProvider());
        }

        private static IAuthenticationManager AuthenticationManagerFactory()
            => HttpContext.Current.GetOwinContext().Authentication;

        private static IPrincipal PrincipalFactory()
            => HttpContext.Current.User;
    }
}