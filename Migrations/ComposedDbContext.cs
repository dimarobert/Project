using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Account.DAL;
using Project.Account.Models;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;
using Project.UserProfileDomain.Models;

namespace Project.Migrations {
    internal class MigrationDbContext : IdentityWithRoleDbContext {

        static MigrationDbContext() {
            Database.SetInitializer<MigrationDbContext>(null);
        }

        public MigrationDbContext()
            : base("name=DefaultConnection") {
        }

        // Story domain
        public DbSet<Story> Stories { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Hashtag> Hashtags { get; set; }


        // UserProfile Domain
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<UserGoal> UserGoals { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new StoryTypeConfiguration());
            modelBuilder.Configurations.Add(new CommentTypeConfiguration());
        }

    }
}
