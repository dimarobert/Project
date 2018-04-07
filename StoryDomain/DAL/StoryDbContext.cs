using Helpers.DbContextHelpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Project.StoryDomain.DAL {
    public class StoryDbContext : BaseDbContext<StoryDbContext>, IStoryDbContext {

        //public DbSet<Models.UserInfoRef> UserReferences { get; set; }
        public DbSet<Models.Story> Stories { get; set; }
        public DbSet<Models.Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }

    }
}
