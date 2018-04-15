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

        public int ParentStoryId { get; set; }

        public int? ParentCommentId { get; set; }

        public string UserId { get; set; }

        public string Text { get; set; }

        public UserInfo User { get; set; }

        public IList<CommentVM> Comments { get; set; }

    }
}