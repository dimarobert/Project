using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Project.Core.Repositories;
using Project.UserProfileDomain.Models;

namespace Project.UserProfileDomain.Repositories {

    public interface IUserProfileRepostory : IEntityRepository<UserProfile> {

        UserProfile GetUserProfile(string userId);

    }

    public class UserProfileRepository : IUserProfileRepostory {

        public IList<UserProfile> All => throw new NotImplementedException();

        public IList<UserProfile> AllIncluding(params Expression<Func<UserProfile, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public void Delete(UserProfile entity) {
            throw new NotImplementedException();
        }

        public IList<UserProfile> Get(params Expression<Func<UserProfile, object>>[] filters) {
            throw new NotImplementedException();
        }

        public IList<UserProfile> GetIncluding(Expression<Func<UserProfile, object>>[] filters, Expression<Func<UserProfile, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public UserProfile GetUserProfile(string userId) {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(UserProfile entity) {
            throw new NotImplementedException();
        }

        public void Save() {
            throw new NotImplementedException();
        }
    }
}
