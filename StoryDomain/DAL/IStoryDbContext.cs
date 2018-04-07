using System.Data.Entity;
using Project.StoryDomain.Models;

namespace Project.StoryDomain.DAL {

    public interface IStoryDbContext {
        DbSet<Reply> CommentReplies { get; set; }
        DbSet<Story> Stories { get; set; }
        DbSet<Comment> StoryComments { get; set; }
        DbSet<UserInfoRef> UserReferences { get; set; }
    }
}