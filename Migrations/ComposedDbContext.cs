using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Account.Models;
using Project.StoryDomain.DAL;
using Project.StoryDomain.Models;
using Project.UserProfileDomain.Models;

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
        public DbSet<Comment> Comments { get; set; }


        // UserProfile Domain
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<UserGoal> UserGoals { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new StoryTypeConfiguration());
            modelBuilder.Configurations.Add(new CommentTypeConfiguration());
        }

    }
}
