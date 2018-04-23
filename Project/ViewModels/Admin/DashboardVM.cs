using Project.ViewModels.Story;
using Project.ViewModels.UserProfile;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels.Admin {
    public class DashboardVM {
        public IList<UserBasicInfoVM> RegularUsers { get; set; }

        public IList<UserBasicInfoVM> Coaches { get; set; }

        public IList<UserBasicInfoVM> Admins { get; set; }

        public IList<InterestVM> Interests { get; set; }

        public IList<GroupVM> Groups { get; set; }

        public IList<HashtagVM> Hashtags { get; set; }
    }
}