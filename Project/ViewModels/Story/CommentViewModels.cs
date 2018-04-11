using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Project.Account.Models;
using Project.ViewModels.Story;

namespace Project.ViewModels.Story
{
    public class CommentViewModels
    {
        public int Id { get; set; }

        public int ParentStoryId { get; set; }

        public int? ParentCommentId { get; set; }

        public string UserId { get; set; }

        public string Text { get; set; }

        public virtual UserInfo User { get; set; }

        [Required]
        public virtual StoryViewModels ParentStory { get; set; }

        [ForeignKey("ParentCommentId")]
        public virtual CommentViewModels ParentComment { get; set; }

        public virtual ICollection<CommentViewModels> Comments { get; set; }

    }
}