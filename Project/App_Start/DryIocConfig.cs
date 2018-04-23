using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using DryIoc;
using DryIoc.Mvc;
using Project.Account.DAL;
using Project.Account.Managers;
using Project.Account.Models;
using Project.Account.Services;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Repositories;
using Project.UserProfileDomain.DAL;
using Project.UserProfileDomain.Repositories;
using Project.Account.Repositories;
using DryIoc.WebApi;
using System.Web.Http;
using Project.StoryDomain.Services;

namespace Project {
    public class DryIocConfig {

        private static Lazy<IContainer> container = new Lazy<IContainer>(() => {
            var container = new Container();
            RegisterTypes(container);
            return container;
        });

        internal static void WithWebApi() {
            var webApiContainer = GetContainer().WithWebApi(GlobalConfiguration.Configuration, throwIfUnresolved: type => DryIocWebApi.IsController(type));
            container = new Lazy<IContainer>(() => webApiContainer);
        }

        internal static void WithMvc() {
            var mvcContainer = GetContainer().WithMvc(throwIfUnresolved: type => DryIocMvc.IsController(type));
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
            container.Register<IUserStore<UserInfo, string>>(Reuse.InWebRequest, Made.Of(() => new ApplicationUserStore(Arg.Of<AccountDbContext>())));

            container.Register<ApplicationRoleManager>(Reuse.InWebRequest);
            container.Register<IRoleStore<RoleInfo, string>>(Reuse.InWebRequest, Made.Of(() => new ApplicationRoleStore(Arg.Of<AccountDbContext>())));

            // Register application types
            container.Register<IPrincipal>(Reuse.InWebRequest, Made.Of(() => PrincipalFactory()));
            container.Register<IUserService, UserService>(Reuse.InWebRequest);

            // Story Domain
            container.Register<IStoryContext, StoryContext>(Reuse.InWebRequest);
            container.Register<IStoryRepository, StoryRepository>(Reuse.InWebRequest);
            container.Register<ICommentRepository, CommentRepository>(Reuse.InWebRequest);
            container.Register<IHashtagRepository, HashtagRepository>(Reuse.InWebRequest);
            container.Register<IGroupRepository, GroupRepository>(Reuse.InWebRequest);
            container.Register<IStoryService, StoryService>(Reuse.Singleton);
            

            container.Register<IStoryUnitOfWork, StoryUnitOfWork>(Reuse.InWebRequest);

            // UserProfile Domain
            container.Register<IUserProfileContext, UserProfileContext>(Reuse.InWebRequest);
            container.Register<IUserProfileRepository, UserProfileRepository>(Reuse.InWebRequest);
            container.Register<IInterestRepository, InterestRepository>(Reuse.InWebRequest);
            container.Register<IGoalRepository, GoalRepository>(Reuse.InWebRequest);

            container.Register<IUserProfileUnitOfWork, UserProfileUnitOfWork>(Reuse.InWebRequest);

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