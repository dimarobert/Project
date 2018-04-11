using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels
{
    public class GroupVM
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public List<UserProfileRefVM> Members { get; set; }
    }
}