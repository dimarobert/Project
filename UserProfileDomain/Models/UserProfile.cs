using Project.Account.Models;
using Project.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.UserProfileDomain.Models {
    public class UserProfile : ObjectWithState, IEntity<int> {

        public int Id { get; set; }

        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string AboutMe { get; set; }

        public DateTime? BannedUntil { get; set; }



        public virtual UserInfo User { get; set; }

        public virtual ICollection<Goal> Goals { get; set; }

        public virtual ICollection<UserInterest> Interests { get; set; }

    }
}
