using Project.Core.Repositories;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Project.StoryDomain.Repositories {

    public interface IHashtagRepository : IEntityRepository<Hashtag, int> {

        Task<Hashtag> FindByValueAsync(string value);

    }


    public class HashtagRepository : EntityRepository<Hashtag, int>, IHashtagRepository {

        IStoryContext storyDbContext => context as IStoryContext;

        public HashtagRepository(IStoryContext storyDbContext) : base(storyDbContext) { }

        public async Task<Hashtag> FindByValueAsync(string value) {
            return await GetQ(h => h.Value == value).FirstOrDefaultAsync();
        }
    }
}
