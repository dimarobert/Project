
namespace Project.StoryDomain.Models {
    public class Reply {

        public int Id { get; set; }

        public int CommentId { get; set; }

        public string UserId { get; set; }


        public string Text { get; set; }


        public virtual Comment Comment { get; set; }

        public virtual UserInfoRef User { get; set; }
    }
}