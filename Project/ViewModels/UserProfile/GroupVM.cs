using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels.UserProfile {
    public class GroupVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<UserProfileRefVM> Members { get; set; }
    }
}