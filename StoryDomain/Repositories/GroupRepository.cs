using Project.Core.DbContext;
using Project.Core.Enums;
using Project.Core.Repositories;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;
using Project.UserProfileDomain.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Project.StoryDomain.Repositories {
    public interface IGroupRepository : IEntityRepository<Group, int> {

        Task<Group> FindByTitleAsync(string groupTitle);
        Task<Group> GetGroupByIdAsync(int id);
        Task<Group> GetGroupByInterestIdAsync(int interestId);
        }

        public class GroupRepository : EntityRepository<Group, int>, IGroupRepository {

        IStoryContext storyContext => context as IStoryContext;

        public GroupRepository(IStoryContext storyContext) : base(storyContext) { }

        public async Task<Group> FindByTitleAsync(string groupTitle) {
            return await GetQ(g => g.Title == groupTitle).FirstOrDefaultAsync();
        }

        public async Task<Group> GetGroupByIdAsync(int id) {
            return await GetQ(g => g.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Group> GetGroupByInterestIdAsync(int interestId) {
            return await GetQ(g => g.InterestId == interestId)
                .Include(u => u.Members.Select(up => up.UserProfile))
                .Include(u => u.Stories)
                .FirstOrDefaultAsync();
        }
    }
}