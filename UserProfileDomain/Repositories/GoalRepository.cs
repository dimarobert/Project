using Project.Core.Repositories;
using Project.UserProfileDomain.DAL;
using Project.UserProfileDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.UserProfileDomain.Repositories
{
    public interface IGoalRepository : IEntityRepository<Goal>
    {
        IList<Goal> GetUserGoals(string userId);

    }
    public class GoalRepository : IGoalRepository
    {
        readonly IUserProfileContext userProfileDbContext;

        public GoalRepository(IUserProfileContext userProfileDbContext)
        {
            this.userProfileDbContext = userProfileDbContext ?? throw new ArgumentNullException(nameof(userProfileDbContext));
        }

        public IList<Goal> All => throw new NotImplementedException();

        public Task<IList<Goal>> AllAsync => throw new NotImplementedException();

        public IList<Goal> AllIncluding(params Expression<Func<Goal, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Goal>> AllIncludingAsync(params Expression<Func<Goal, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void Delete(Goal entity)
        {
            throw new NotImplementedException();
        }

        public IList<Goal> Get(params Expression<Func<Goal, bool>>[] filters)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Goal>> GetAsync(params Expression<Func<Goal, bool>>[] filters)
        {
            throw new NotImplementedException();
        }

        public IList<Goal> GetIncluding(Expression<Func<Goal, bool>>[] filters, Expression<Func<Goal, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Goal>> GetIncludingAsync(Expression<Func<Goal, bool>>[] filters, Expression<Func<Goal, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IList<Goal> GetUserGoals(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException($"Could not retrieve user goals. Invalid parameter value: '{userId}'.", nameof(userId));
            }

            return userProfileDbContext.Goals.Where(ui => ui.UserProfile.UserId == userId).ToList();
        }

        public void InsertOrUpdate(Goal entity)
        {
            userProfileDbContext.Goals.Add(entity);
        }

        public void InsertOrUpdateGraph(Goal entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            userProfileDbContext.SaveChanges();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
