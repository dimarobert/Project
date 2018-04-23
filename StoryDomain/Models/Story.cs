using Project.Account.Models;
using Project.Core.Enums;
using Project.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Project.StoryDomain.Models {
    public class Story : ObjectWithState, IEntity<int> {

        public int Id { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public StoryType Type { get; set; }

        public DateTime? Date { get; set; }

        public virtual UserInfo User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
    }
}
