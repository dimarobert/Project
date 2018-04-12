using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Account.Models;

namespace Project.ViewModels.Story
{
    public class LikeVM
    {
        public int Id { get; set; }

        public int StoryId { get; set; }

        public string UserId { get; set; }

        public DateTime Date { get; set; }

        public virtual UserInfo User { get; set; }

        public virtual StoryVM Story { get; set; }
    }
}