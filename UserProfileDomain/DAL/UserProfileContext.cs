using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Project.Account.DAL;
using Project.UserProfileDomain.Models;
using Project.Core.DbContext;

namespace Project.UserProfileDomain.DAL {
    public interface IUserProfileContext {

        DbSet<Interest> Interests { get; set; }
        DbSet<Goal> Hobbies { get; set; }

        DbSet<UserInterest> UserInterests { get; set; }
        DbSet<UserGoal> UserHobbies { get; set; }

        DbSet<Notification> Notifications { get; set; }

        DbSet<UserProfile> UserProfiles { get; set; }

    }

    public class UserProfileContext : BaseDbContext<UserProfileContext>, IUserProfileContext {

        public DbSet<Interest> Interests { get; set; }
        public DbSet<Goal> Hobbies { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<UserGoal> UserHobbies { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

    }
}
