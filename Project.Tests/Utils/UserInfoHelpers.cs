using AutoFixture;
using Project.Account.Models;
using Project.Core.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Tests.Utils {
    public static class UserInfoHelpers {

        public static IEnumerable<UserProfileDomain.Models.UserProfile> CreateUsers(this IFixture fixture, int count, Func<IList<StandardRoles>> roles) {
            return fixture.Build<UserProfileDomain.Models.UserProfile>()
                .Without(p => p.User)
                .Do(p => p.User = fixture.CreateUser(roles()))
                .CreateMany(count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="roles">The roles that will be set to the mock user. Leave null for default (Normal) role.</param>
        /// <returns></returns>
        public static UserInfo CreateUser(this IFixture fixture, IList<StandardRoles> roles = null) {
            if (roles == null)
                roles = new List<StandardRoles> { StandardRoles.Normal };
            return fixture.Build<UserInfo>()
                 .Do(u => {
                     foreach (var role in roles)
                         u.Roles.Add(fixture.CreateRole(role));
                 })
                 .Create();
        }

        public static UserRoleInfo CreateRole(this IFixture fixture, StandardRoles role) {
            return fixture.Build<UserRoleInfo>()
                .With(ur => ur.Role, fixture.Build<RoleInfo>()
                    .With(r => r.Name, role.ToString())
                    .Create())
                .Create();
        }

    }
}
