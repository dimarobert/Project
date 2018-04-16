using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels.Admin
{
    public class DashboardVM
    {
        public IList<UserProfileRefVM> RegularUsers { get; set; }

        public IList<UserProfileRefVM> Coaches { get; set; }

        public IList<GroupVM> Groups { get; set; }

        public IList<InterestVM> Interests { get; set; }

    }
}