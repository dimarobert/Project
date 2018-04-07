using Project.StoryDomain.Models;
using System.Data.Entity.ModelConfiguration;

namespace Project.StoryDomain.DAL {
    public class CommentTypeConfiguration : EntityTypeConfiguration<Comment> {

        public CommentTypeConfiguration() {
            HasMany(c => c.Comments)
                .WithOptional()
                .HasForeignKey(c => c.ParentCommentId);

        }
    }
}
