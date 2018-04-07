using Project.Core.Repositories;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.StoryDomain.Repositories {
    public interface IStoryRepository : IEntityRepository<Story> {
        IList<Story> GetUserStories(string userId);

    }

    public class StoryRepository : IStoryRepository {

        IStoryContext storyDbContext { get; }

        public IList<Story> All => throw new NotImplementedException();

        public StoryRepository(IStoryContext storyDbContext) {
            this.storyDbContext = storyDbContext ?? throw new ArgumentNullException(nameof(storyDbContext));
        }

        public IList<Story> GetUserStories(string userId) {
            if (string.IsNullOrWhiteSpace(userId) || storyDbContext.Stories == null)
                return new List<Story>();

            return storyDbContext.Stories.Where(story => story.UserId == userId).ToList();
        }

        public IList<Story> AllIncluding(params Expression<Func<Story, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public IList<Story> Get(params Expression<Func<Story, object>>[] filters) {
            throw new NotImplementedException();
        }

        public IList<Story> GetIncluding(Expression<Func<Story, object>>[] filters, Expression<Func<Story, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(Story entity) {
            throw new NotImplementedException();
        }

        public void Save() {
            throw new NotImplementedException();
        }

        public void Delete(Story entity) {
            throw new NotImplementedException();
        }
    }
}
