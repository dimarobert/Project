using Project.ViewModels.Admin;
using Project.ViewModels.Story;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.ViewModels.UserProfile {
    public class GroupVM
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public IList<UserBasicInfoVM> Members { get; set; }

        public IList<StoryVM> RegularStories { get; set; }

        public IList<StoryVM> PromotedRegularStories { get; set; }

        public IList<StoryVM> GivingAdviceStories { get; set; }

        public IList<StoryVM> PromotedGivingAdviceStories { get; set; }

        public IList<StoryVM> AskingAdviceStories { get; set; }

        public IList<StoryVM> PromotedAskingAdviceStories { get; set; }

        public int CurrentPage { get; set; }

        public int NoOfPages { get; set; }

    }
}