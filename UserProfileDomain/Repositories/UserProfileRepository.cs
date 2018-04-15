using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Project.Account.Models;
using Project.Core.Account;
using Project.Core.DbContext;
using Project.Core.Repositories;
using Project.UserProfileDomain.DAL;
using Project.UserProfileDomain.Models;

namespace Project.UserProfileDomain.Repositories {

    public interface IUserProfileRepository : IEntityRepository<UserProfile> {

        UserProfile GetUserProfile(string userId);
        Task<UserProfile> GetUserProfileAsync(string userId);

        void CreateProfile(UserInfo user);
        Task CreateProfileAsync(UserInfo user);

        Task<IList<UserProfile>> GetUsersInRoleProfileAsync(string roleName);
        Task<IList<UserProfile>> GetUsersInRoleProfileAsync(StandardRoles role);

        /// <summary>
        /// Gets the <see cref="UserProfile"/> for users that exactly matches the role. e.g. if a request for <see cref="StandardRoles.Coach"/> is made, users in role <see cref="StandardRoles.Admin"/> will not be retrieved.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IList<UserProfile>> GetStrictInRoleUserProfilesAsync(StandardRoles role);

    }

    public class UserProfileRepository : IUserProfileRepository {

        readonly IUserProfileContext context;

        public UserProfileRepository(IUserProfileContext userProfileContext) {
            context = userProfileContext;
        }

        public IList<UserProfile> All => GetQ().ToList();

        public Task<IList<UserProfile>> AllAsync => Task.Run<IList<UserProfile>>(async () => await GetQ().ToListAsync());

        public IList<UserProfile> AllIncluding(params Expression<Func<UserProfile, object>>[] includeProperties) {
            return GetIncludingQ(includeProperties).ToList();
        }

        public async Task<IList<UserProfile>> AllIncludingAsync(params Expression<Func<UserProfile, object>>[] includeProperties) {
            return await GetIncludingQ(includeProperties).ToListAsync();
        }

        public IList<UserProfile> Get(params Expression<Func<UserProfile, bool>>[] filters) {
            return GetQ(filters).ToList();
        }

        public async Task<IList<UserProfile>> GetAsync(params Expression<Func<UserProfile, bool>>[] filters) {
            return await GetQ(filters).ToListAsync();
        }

        public IList<UserProfile> GetIncluding(Expression<Func<UserProfile, bool>>[] filters, Expression<Func<UserProfile, object>>[] includeProperties) {
            return GetIncludingQ(GetQ(filters), includeProperties).ToList();
        }

        public async Task<IList<UserProfile>> GetIncludingAsync(Expression<Func<UserProfile, bool>>[] filters, Expression<Func<UserProfile, object>>[] includeProperties) {
            return await GetIncludingQ(GetQ(filters), includeProperties).ToListAsync();
        }

        public UserProfile GetUserProfile(string userId) {
            return GetQ(profile => profile.UserId == userId).FirstOrDefault();
        }

        public async Task<UserProfile> GetUserProfileAsync(string userId) {
            return await GetQ(profile => profile.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<IList<UserProfile>> GetStrictInRoleUserProfilesAsync(StandardRoles role) {
            var query = GetUsersInRoleProfileQAsync(role.ToString());

            if (role < StandardRoles.Admin)
                query = FilterNotInRoleQAsync(query, StandardRoles.Admin.ToString());

            if(role < StandardRoles.Coach)
                query = FilterNotInRoleQAsync(query, StandardRoles.Coach.ToString());

            return await query.ToListAsync();
        }

        public async Task<IList<UserProfile>> GetUsersInRoleProfileAsync(StandardRoles role) {
            return await GetUsersInRoleProfileAsync(role.ToString());
        }

        public async Task<IList<UserProfile>> GetUsersInRoleProfileAsync(string roleName) {
            return await GetUsersInRoleProfileQAsync(roleName).ToListAsync();

            //var sql = (query as System.Data.Entity.Infrastructure.DbQuery<UserProfile>).Sql;
        }

        public async Task CreateProfileAsync(UserInfo user) {
            if (await GetUserProfileAsync(user.Id) != null)
                return;

            var profile = new UserProfile {
                UserId = user.Id
            };

            InsertOrUpdate(profile);
            await SaveAsync();
        }

        public void CreateProfile(UserInfo user) {

            if (GetUserProfile(user.Id) != null)
                return;

            var profile = new UserProfile {
                UserId = user.Id
            };

            InsertOrUpdate(profile);
            Save();
        }

        public void InsertOrUpdateGraph(UserProfile entityGraph) {
            context.UserProfiles.Add(entityGraph);

            if (entityGraph.State != Core.Models.ModelState.Added)
                context.ApplyStateChanges();
        }

        public void InsertOrUpdate(UserProfile entity) {
            if (entity.Id == default(int))
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
        }

        public void Save() {
            context.SaveChanges();
        }

        public async Task SaveAsync() {
            await context.SaveChangesAsync();
        }

        public void Delete(UserProfile entity) {
            context.Entry(new UserProfile {
                Id = entity.Id
            }).State = EntityState.Deleted;
        }

        private IQueryable<UserProfile> GetQ(params Expression<Func<UserProfile, bool>>[] filters) {
            return GetQ(context.UserProfiles, filters);
        }

        private IQueryable<UserProfile> GetQ(IQueryable<UserProfile> query, params Expression<Func<UserProfile, bool>>[] filters) {
            foreach (var filter in filters)
                query = query.Where(filter);

            return query;
        }

        private IQueryable<UserProfile> GetIncludingQ(params Expression<Func<UserProfile, object>>[] includeProperties) {
            return GetIncludingQ(context.UserProfiles, includeProperties);
        }

        private IQueryable<UserProfile> GetIncludingQ(IQueryable<UserProfile> query, params Expression<Func<UserProfile, object>>[] includeProperties) {
            foreach (var include in includeProperties)
                query = query.Include(include);

            return query;
        }


        private IQueryable<UserProfile> GetUsersInRoleProfileQAsync(string roleName) {
            return context.UserProfiles.Where(p => p.User.Roles.Any(r => r.Role.Name == roleName));
        }

        private IQueryable<UserProfile> FilterNotInRoleQAsync(IQueryable<UserProfile> query, string roleName) {
            return query.Where(p => !p.User.Roles.Any(r => r.Role.Name == roleName));
        }
    }
}
