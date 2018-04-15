using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Repositories;
using Project.UserProfileDomain.DAL;
using Project.UserProfileDomain.Models;

namespace Project.UserProfileDomain.Repositories {

    public interface IInterestRepository : IEntityRepository<Interest> {

        IList<UserInterest> GetUserInterests(string userId);

    }

    public class InterestRepository : IInterestRepository {

        readonly IUserProfileContext userProfileDbContext;

        public InterestRepository(IUserProfileContext userProfileDbContext) {
            this.userProfileDbContext = userProfileDbContext ?? throw new ArgumentNullException(nameof(userProfileDbContext));
        }

        public IList<Interest> All => throw new NotImplementedException();

        public Task<IList<Interest>> AllAsync => Task.Run<IList<Interest>>(async () => await userProfileDbContext.Interests.ToListAsync());

        public IList<Interest> AllIncluding(params Expression<Func<Interest, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public Task<IList<Interest>> AllIncludingAsync(params Expression<Func<Interest, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public void Delete(Interest entity) {
            throw new NotImplementedException();
        }

        public IList<Interest> Get(params Expression<Func<Interest, bool>>[] filters) {
            throw new NotImplementedException();
        }

        public Task<IList<Interest>> GetAsync(params Expression<Func<Interest, bool>>[] filters) {
            throw new NotImplementedException();
        }

        public IList<Interest> GetIncluding(Expression<Func<Interest, bool>>[] filters, Expression<Func<Interest, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public Task<IList<Interest>> GetIncludingAsync(Expression<Func<Interest, bool>>[] filters, Expression<Func<Interest, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public IList<UserInterest> GetUserInterests(string userId) {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException($"Could not retrieve user interests. Invalid parameter value: '{userId}'.", nameof(userId));

            return userProfileDbContext.UserInterests.Where(ui => ui.UserProfile.UserId == userId).ToList();
        }

        public void InsertOrUpdate(Interest entity) {
            throw new NotImplementedException();
        }

        public void InsertOrUpdateGraph(Interest entity) {
            throw new NotImplementedException();
        }

        public void Save() {
            throw new NotImplementedException();
        }

        public Task SaveAsync() {
            throw new NotImplementedException();
        }
    }
}
