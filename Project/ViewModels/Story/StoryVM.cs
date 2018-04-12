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

        public virtual UserInfo User { get; set; }

        public virtual ICollection<CommentVM> Comments { get; set; }

        public virtual ICollection<LikeVM> Likes { get; set; }
    }
}