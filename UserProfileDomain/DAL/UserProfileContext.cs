using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Project.Account.DAL;
using Project.UserProfileDomain.Models;

namespace Project.UserProfileDomain.DAL {
    public interface IUserProfileContext {

        DbSet<Interest> Interests { get; set; }
        DbSet<Hobby> Hobbies { get; set; }

        DbSet<UserInterest> UserInterests { get; set; }
        DbSet<UserHobby> UserHobbies { get; set; }

    }

    public class UserProfileContext : IdentityReferenceDbContext<UserProfileContext>, IUserProfileContext {

        public DbSet<Interest> Interests { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<UserHobby> UserHobbies { get; set; }

    }
}
