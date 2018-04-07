using System.Data.Entity;
using Project.StoryDomain.Models;

namespace Project.StoryDomain.DAL {

    public interface IStoryDbContext {

        DbSet<Story> Stories { get; set; }
        DbSet<Comment> Comments { get; set; }

    }
}