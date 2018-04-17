using Project.Core.Repositories;
using Project.UserProfileDomain.DAL;
using Project.UserProfileDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.UserProfileDomain.Repositories {
    public interface IGoalRepository : IEntityRepository<Goal, int> {
        IList<Goal> GetUserGoals(string userId);

    }

    public class GoalRepository : EntityRepository<Goal, int>, IGoalRepository {

        IUserProfileContext userProfileContext => context as IUserProfileContext;

        public GoalRepository(IUserProfileContext userProfileContext) : base(userProfileContext) { }

        public IList<Goal> GetUserGoals(string userId) {
            if (string.IsNullOrWhiteSpace(userId)) {
                throw new ArgumentException($"Could not retrieve user goals. Invalid parameter value: '{userId}'.", nameof(userId));
            }

            return userProfileContext.Goals.Where(ui => ui.UserProfile.UserId == userId).ToList();
        }
       
    }
}
