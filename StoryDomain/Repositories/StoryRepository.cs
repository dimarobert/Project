using Project.Core.Enums;
using Project.Core.Repositories;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.StoryDomain.Repositories {
    public interface IStoryRepository : IEntityRepository<Story, int> {
        IList<Story> GetUserStories(string userId);

        Task<IList<Story>> GetUserStoriesAsync(string userId);

        Task<Story> GetStoryById(int storyId);
        Task<IList<Comment>> GetAllComments(int id);
        Task<IList<Story>> GetPromotedStoriesByGroupAndTypeAsync(int groupId, StoryType type);

        Task<IList<Story>> GetAllStoriesByGroupAndTypeAsync(int groupId, StoryType type);

        Task<IList<Story>> GetUnpromotedStoriesByGroupAndTypeAsync(int groupId, StoryType type);

    }

    public class StoryRepository : EntityRepository<Story, int>, IStoryRepository {
        private readonly int promotedStoryLikeNumber = 5;
        IStoryContext storyDbContext => context as IStoryContext;

        public StoryRepository(IStoryContext storyDbContext) : base(storyDbContext) { }

        public IList<Story> GetUserStories(string userId) {
            if (string.IsNullOrWhiteSpace(userId))
                return new List<Story>();

            return GetQ(story => story.UserId == userId).ToList();
        }

        public async Task<IList<Story>> GetUserStoriesAsync(string userId) {
            if (string.IsNullOrWhiteSpace(userId) || storyDbContext.Stories == null)
                return new List<Story>();

            return await GetQ(story => story.UserId == userId).ToListAsync();
        }

        public async Task<Story> GetStoryById(int storyId) {
            return await GetQ(s => s.Id == storyId).FirstOrDefaultAsync();
        }

        public async Task<IList<Story>> GetPromotedStoriesByGroupAndTypeAsync(int groupId, StoryType type) {
            return await GetQ(s => s.GroupId == groupId && s.Type == type && s.Likes.Count >= promotedStoryLikeNumber)
                .ToListAsync();
        }

        public async Task<IList<Story>> GetAllStoriesByGroupAndTypeAsync(int groupId, StoryType type) {
            return await GetQ(s => s.GroupId == groupId && s.Type == type).ToListAsync();
        }

        public async Task<IList<Story>> GetUnpromotedStoriesByGroupAndTypeAsync(int groupId, StoryType type) {
            return await GetQ(s => s.GroupId == groupId && s.Type == type && s.Likes.Count < promotedStoryLikeNumber)
                .ToListAsync();
        }

        public async Task<IList<Comment>> GetAllComments(int id) {
            var story = await GetIncludingQ(GetQ(s => s.Id == id), s => s.Comments).FirstOrDefaultAsync();

            if (story == null)
                return new List<Comment>();

            var result = new List<Comment>();
            foreach (var comment in story.Comments) {
                await GetRepliesRecursive(comment.Id, result);
                result.Add(comment);
            }
            return result;
        }

        private async Task GetRepliesRecursive(int parentCommentId, List<Comment> accumulator) {
            var replies = await storyDbContext.Comments.Where(c => c.ParentCommentId == parentCommentId).Include(c => c.Comments).ToListAsync();

            foreach (var reply in replies) {
                if (reply.Comments.Any()) {
                    await GetRepliesRecursive(reply.Id, accumulator);
                }
                accumulator.Add(reply);
            }
        }
    }
}
