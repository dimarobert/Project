using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Account.Models;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;

namespace Project.Migrations {
    internal class MigrationDbContext : IdentityDbContext<UserInfo> {

        static MigrationDbContext() {
            Database.SetInitializer<MigrationDbContext>(null);
        }

        public MigrationDbContext()
            : base("name=DefaultConnection", throwIfV1Schema: true) {
        }

        // Story domain
        public DbSet<Story> Stories { get; set; }
        public DbSet<Comment> StoryComments { get; set; }
        public DbSet<Reply> CommentReplies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserInfoRefMap());

        }

    }
}
