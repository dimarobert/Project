using Project.ViewModels.UserProfile;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels.Admin {
    public class DashboardVM {
        public IList<UserBasicInfoVM> RegularUsers { get; set; }

        public IList<UserBasicInfoVM> Coaches { get; set; }

        public IList<GroupVM> Groups { get; set; }

        public IList<InterestVM> Interests { get; set; }

    }
}