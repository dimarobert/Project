using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Project.Core.Repositories;
using Project.UserProfileDomain.DAL;
using Project.UserProfileDomain.Models;

namespace Project.UserProfileDomain.Repositories {

    public interface IHobbyRepository: IEntityRepository<Hobby> {

        IList<UserInterest> GetUserInterests(string userId);

    }

    public class HobbyRepository : IHobbyRepository {

        readonly IUserProfileContext userProfileDbContext;

        public IList<Hobby> All => throw new NotImplementedException();

        public HobbyRepository(IUserProfileContext userProfileDbContext) {
            this.userProfileDbContext = userProfileDbContext ?? throw new ArgumentNullException(nameof(userProfileDbContext));
        }

        public IList<UserInterest> GetUserInterests(string userId) {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException($"Could not retrieve user interests. Invalid parameter value: '{userId}'.", nameof(userId));

            return userProfileDbContext.UserInterests.Where(ui => ui.UserId == userId).ToList();
        }

        public IList<Hobby> AllIncluding(params Expression<Func<Hobby, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public void Delete(Hobby entity) {
            throw new NotImplementedException();
        }

        public IList<Hobby> Get(params Expression<Func<Hobby, object>>[] filters) {
            throw new NotImplementedException();
        }

        public IList<Hobby> GetIncluding(Expression<Func<Hobby, object>>[] filters, Expression<Func<Hobby, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(Hobby entity) {
            throw new NotImplementedException();
        }

        public void Save() {
            throw new NotImplementedException();
        }
    }

}
