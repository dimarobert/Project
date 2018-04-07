using Project.Account.DAL;
using Project.StoryDomain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Project.StoryDomain.DAL {

    public interface IStoryContext {

        DbSet<Story> Stories { get; set; }
        DbSet<Comment> Comments { get; set; }

    }

    public class StoryContext : IdentityReferenceDbContext<StoryContext>, IStoryContext {

        //public DbSet<Models.UserInfoRef> UserReferences { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new StoryTypeConfiguration());
            modelBuilder.Configurations.Add(new CommentTypeConfiguration());
        }

    }
}
