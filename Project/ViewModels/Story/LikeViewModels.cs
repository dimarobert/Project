using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Account.Models;

namespace Project.ViewModels.Story
{
    public class LikeViewModels
    {
        public int Id { get; set; }

        public int StoryId { get; set; }

        public string UserId { get; set; }

        public DateTime Date { get; set; }

        public virtual UserInfo User { get; set; }

        public virtual StoryViewModels Story { get; set; }
    }
}