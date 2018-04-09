using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Project.Core.Repositories;
using Project.UserProfileDomain.DAL;
using Project.UserProfileDomain.Models;

namespace Project.UserProfileDomain.Repositories {

    public interface IHobbyRepository: IEntityRepository<Goal> {

        IList<UserInterest> GetUserInterests(string userId);

    }

    public class HobbyRepository : IHobbyRepository {

        readonly IUserProfileContext userProfileDbContext;

        public IList<Goal> All => throw new NotImplementedException();

        public HobbyRepository(IUserProfileContext userProfileDbContext) {
            this.userProfileDbContext = userProfileDbContext ?? throw new ArgumentNullException(nameof(userProfileDbContext));
        }

        public IList<UserInterest> GetUserInterests(string userId) {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException($"Could not retrieve user interests. Invalid parameter value: '{userId}'.", nameof(userId));

            return userProfileDbContext.UserInterests.Where(ui => ui.UserId == userId).ToList();
        }

        public IList<Goal> AllIncluding(params Expression<Func<Goal, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public void Delete(Goal entity) {
            throw new NotImplementedException();
        }

        public IList<Goal> Get(params Expression<Func<Goal, object>>[] filters) {
            throw new NotImplementedException();
        }

        public IList<Goal> GetIncluding(Expression<Func<Goal, object>>[] filters, Expression<Func<Goal, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(Goal entity) {
            throw new NotImplementedException();
        }

        public void Save() {
            throw new NotImplementedException();
        }
    }

}
