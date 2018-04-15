using Project.Account.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.ViewModels.Story
{
    public class StoryVM
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public IList<CommentVM> Comments { get; set; }

        public IList<LikeVM> Likes { get; set; }
    }
}