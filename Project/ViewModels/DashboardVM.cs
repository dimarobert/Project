using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels.Admin
{
    public class DashboardVM
    {
        public List<UserProfileRefVM> RegularUsers { get; set; }

        public List<UserProfileRefVM> Coaches { get; set; }

        public List<GroupVM> Groups { get; set; }

        public List<InterestVM> Interests { get; set; }

    }
}