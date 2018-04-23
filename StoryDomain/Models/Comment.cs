using Project.Account.Models;
using Project.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.StoryDomain.Models {

    [Table("Comments")]
    public class Comment : ObjectWithState, IEntity<int> {
        public int Id { get; set; }

        [Required]
        public int ParentStoryId { get; set; }

        public int? ParentCommentId { get; set; }

        public string UserId { get; set; }

        public string Text { get; set; }

        public DateTime? Date { get; set; }

        public virtual UserInfo User { get; set; }

        public virtual Story ParentStory { get; set; }

        public virtual Comment ParentComment { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}