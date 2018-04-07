using Project.Account.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.StoryDomain.Models {

    [Table("Comments")]
    public class Comment {
        public int Id { get; set; }

        [Required]
        public int ParentStoryId { get; set; }

        public int? ParentCommentId { get; set; }

        public string UserId { get; set; }


        public string Text { get; set; }


        public virtual UserInfo User { get; set; }


        [ForeignKey("ParentStoryId")]
        [Required]
        public virtual Story ParentStory { get; set; }

        [ForeignKey("ParentCommentId")]
        public virtual Comment ParentComment { get; set; }
    }
}