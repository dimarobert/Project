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

    public interface IInterestRepository : IEntityRepository<Interest, int> {

        IList<UserInterest> GetUserInterests(string userId);

    }

    public class InterestRepository : EntityRepository<Interest, int>, IInterestRepository {

        IUserProfileContext userProfileDbContext => context as IUserProfileContext;

        public InterestRepository(IUserProfileContext userProfileDbContext) : base(userProfileDbContext) { }

        public IList<UserInterest> GetUserInterests(string userId) {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException($"Could not retrieve user interests. Invalid parameter value: '{userId}'.", nameof(userId));

            return userProfileDbContext.UserInterests.Where(ui => ui.UserProfile.UserId == userId).ToList();
        }

    }
}
