using Project.ViewModels.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels.UserProfile {
    public class GroupVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IList<UserProfileRefVM> Members { get; set; }

        public IList<StoryVM> RegularStories { get; set; }

        public IList<StoryVM> GivingAdviceStories { get; set; }

        public IList<StoryVM> AskingAdviceStories { get; set; }


    }
}