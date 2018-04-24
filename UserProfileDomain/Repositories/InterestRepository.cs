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

        IList<UserInterest> GetUserInterests(int userProfileId);
        IList<Interest> GetAllForUser(int userProfileId, bool excludeAlreadyAdded = false);
        Interest GetInterest(int interestId);
    }

    public class InterestRepository : EntityRepository<Interest, int>, IInterestRepository {

        IUserProfileContext userProfileDbContext => context as IUserProfileContext;

        public InterestRepository(IUserProfileContext userProfileDbContext) : base(userProfileDbContext) { }

        public IList<UserInterest> GetUserInterests(int userProfileId) {
            return userProfileDbContext.UserInterests.Where(ui => ui.UserProfileId == userProfileId).ToList();
        }

        public IList<Interest> GetAllForUser(int userProfileId, bool excludeAlreadyAdded = false) {
            return userProfileDbContext.Interests
                .Where(i => 
                    !userProfileDbContext.UserInterests
                        .Where(ui => ui.UserProfileId == userProfileId && ui.InterestId == i.Id)
                        .Any()
                ).ToList();
        }

        public Interest GetInterest(int interestId) {
            return userProfileDbContext.Interests.Where(i => i.Id == interestId).FirstOrDefault();
        }
    }
}
