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
    public class CommentVM
    {
        public int Id { get; set; }

        public int? ParentStoryId { get; set; }

        public int? ParentCommentId { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime? Date { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public UserInfo User { get; set; }

        public IList<CommentVM> Comments { get; set; }

    }
}