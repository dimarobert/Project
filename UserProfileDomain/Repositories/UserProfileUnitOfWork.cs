using Project.Core.DbContext;
using Project.Core.Repositories;
using Project.UserProfileDomain.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.UserProfileDomain.Repositories {

    public interface IUserProfileUnitOfWork : IUnitOfWork {
        IUserProfileRepository UserProfiles { get; }
        IInterestRepository Interests { get; }
        IGoalRepository Goals { get; }
    }

    public class UserProfileUnitOfWork : UnitOfWork, IUserProfileUnitOfWork {
        IUserProfileContext userProfileContext => context as IUserProfileContext;

        public UserProfileUnitOfWork(IUserProfileContext context,
            IUserProfileRepository userProfiles,
            IInterestRepository interests,
            IGoalRepository goals
            ) : base(context) {

            this.context = context;
            UserProfiles = userProfiles;
            Interests = interests;
            Goals = goals;
        }

        public IUserProfileRepository UserProfiles { get; }
        public IInterestRepository Interests { get; }
        public IGoalRepository Goals { get; }
    }

}
