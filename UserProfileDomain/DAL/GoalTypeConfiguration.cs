using Project.UserProfileDomain.Models;
using System.Data.Entity.ModelConfiguration;

namespace Project.UserProfileDomain.DAL {
    public class GoalTypeConfiguration : EntityTypeConfiguration<Goal> {

        public GoalTypeConfiguration() {
            HasRequired(g => g.UserProfile)
                .WithMany(g => g.Goals)
                .HasForeignKey(g => g.UserProfileId)
                .WillCascadeOnDelete();

        }
    }
}
