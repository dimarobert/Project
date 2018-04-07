using System.Collections.Generic;

namespace Project.StoryDomain.Models {
    public class Comment {

        public int Id { get; set; }

        public int StroyId { get; set; }

        public string UserId { get; set; }


        public virtual Story Story { get; set; }

        public virtual UserInfoRef User { get; set; }

    }
}