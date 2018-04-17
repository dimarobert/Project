using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Project.Account.DAL;
using Project.UserProfileDomain.Models;
using Project.Core.DbContext;
using System.Threading.Tasks;

namespace Project.UserProfileDomain.DAL {
    public interface IUserProfileContext : IDbContext {

        DbSet<Interest> Interests { get; set; }

        DbSet<Goal> Goals { get; set; }

        DbSet<Step> Steps { get; set; }

        DbSet<UserInterest> UserInterests { get; set; }
  
        DbSet<Notification> Notifications { get; set; }

        DbSet<UserProfile> UserProfiles { get; set; }

    }

    public class UserProfileContext : IdentityReferenceDbContext<UserProfileContext>, IUserProfileContext {

        public DbSet<Interest> Interests { get; set; }

        public DbSet<Goal> Goals { get; set; }

        public DbSet<Step> Steps { get; set; }

        public DbSet<UserInterest> UserInterests { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new GoalTypeConfiguration());
        }
    }
}
