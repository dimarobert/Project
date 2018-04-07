using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.StoryDomain.Repositories {
    public interface IStoryRepository {
        IList<Story> GetUserStories(string userId);

    }

    public class StoryRepository : IStoryRepository {

        IStoryDbContext StoryDbContext { get; }

        public StoryRepository(IStoryDbContext storyDbContext) {
            StoryDbContext = storyDbContext ?? throw new ArgumentNullException(nameof(storyDbContext));
        }

        public IList<Story> GetUserStories(string userId) {
            if (string.IsNullOrWhiteSpace(userId) || StoryDbContext.Stories == null)
                return new List<Story>();

            return StoryDbContext.Stories.Where(story => story.UserId == userId).ToList();
        }

    }
}
