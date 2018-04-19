using Project.Core.Models;
using Project.UserProfileDomain.Models;

namespace Project.StoryDomain.Models {
    public class GroupMember : ObjectWithState, IEntity<int> {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }


        public int UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}
