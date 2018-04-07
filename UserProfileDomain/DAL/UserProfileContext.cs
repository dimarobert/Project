using Project.Core.DbContext;
using Project.UserProfileDomain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.UserProfileDomain.DAL {
    public interface IUserProfileContext {

        DbSet<Interest> Interests { get; set; }
        DbSet<Hobby> Hobbies { get; set; }

        DbSet<UserInterest> UserInterests { get; set; }
        DbSet<UserHobby> UserHobbies { get; set; }

    }

    public class UserProfileContext : BaseDbContext<UserProfileContext>, IUserProfileContext {

        public DbSet<Interest> Interests { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<UserHobby> UserHobbies { get; set; }

    }
}
