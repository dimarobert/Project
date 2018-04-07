using Project.StoryDomain.Models;
using System.Data.Entity.ModelConfiguration;

namespace Project.StoryDomain.DAL {
    public class CommentTypeConfiguration : EntityTypeConfiguration<Comment> {

        public CommentTypeConfiguration() {
            HasMany(s => s.Comments)
                .WithOptional()
                .HasForeignKey(c => c.ParentCommentId);

        }
    }
}
