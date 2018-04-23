using Project.StoryDomain.Models;
using Project.UserProfileDomain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Project.StoryDomain.DAL {
    public class StoryTypeConfiguration : EntityTypeConfiguration<Story> {

        public StoryTypeConfiguration() {
            HasMany(s => s.Comments)
                .WithOptional(c => c.ParentStory)
                .HasForeignKey(c => c.ParentStoryId);

            HasOptional(s => s.Group)
                .WithMany(g => g.Stories)
                .HasForeignKey(s => s.GroupId);
            
        }
    }
}
