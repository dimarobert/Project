using Project.Account.Models;
using Project.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.ViewModels.Story
{
    public class CreateStoryVM : StoryVM {

        public CreateStoryVM(bool isCoach) {
            IsCoach = isCoach;
        }

        public bool IsCoach { get; set; }
    }

    public class StoryVM
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public StoryType Type { get; set; }

        public DateTime? Date { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }
        
        public int? GroupId { get; set; }

        public IList<CommentVM> Comments { get; set; }

        public IList<LikeVM> Likes { get; set; }

        public bool CanCurrentUserLike { get; set; }

        public bool DidCurrentUserliked { get; set; }

        public string GetTypeString() {
            switch (Type) {
                case StoryType.Regular:
                    return "Story";

                case StoryType.AskingAdvice:
                    return "Asking Advice";

                case StoryType.GivingAdvice:
                    return "Giving Advice";

                default:
                    return "";
            }
        }
    }
}