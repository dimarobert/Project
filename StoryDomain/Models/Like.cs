using Project.Account.Models;
using Project.Core.Models;
using System;

namespace Project.StoryDomain.Models {
    public class Like : ObjectWithState {
        public int Id { get; set; }

        public int StoryId { get; set; }

        public string UserId { get; set; }

        public DateTime Date { get; set; }

        public virtual UserInfo User { get; set; }

        public virtual Story Story { get; set; }
    }
}
