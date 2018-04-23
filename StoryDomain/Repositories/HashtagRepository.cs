using Project.Core.Repositories;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Project.StoryDomain.Repositories {

    public interface IHashtagRepository : IEntityRepository<Hashtag, int> {

        Task<Hashtag> FindByValueAsync(string value);
        Task UpdateHashtags(IList<string> hashtags);
    }


    public class HashtagRepository : EntityRepository<Hashtag, int>, IHashtagRepository {

        IStoryContext storyDbContext => context as IStoryContext;

        public HashtagRepository(IStoryContext storyDbContext) : base(storyDbContext) { }

        public async Task<Hashtag> FindByValueAsync(string value) {
            return await GetQ(h => h.Value == value).FirstOrDefaultAsync();
        }

        public async Task UpdateHashtags(IList<string> hashtags) {
            hashtags = hashtags.Select(h => h.ToLower()).ToList();

            var existingHashtags = await GetAsync(h => hashtags.Contains(h.Value));

            foreach (var hashtag in hashtags) {

                var hashtagModel = existingHashtags.FirstOrDefault(h => h.Value == hashtag);
                if (hashtagModel == null) {
                    hashtagModel = new Hashtag {
                        Value = hashtag,
                        UsageCount = 1,
                        State = Core.Models.ModelState.Added
                    };
                } else {
                    hashtagModel.UsageCount++;
                    hashtagModel.State = Core.Models.ModelState.Modified;
                }

                InsertOrUpdate(hashtagModel);
            }
        }
    }
}
