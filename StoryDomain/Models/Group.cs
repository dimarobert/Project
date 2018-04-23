using Project.Core.Models;
using Project.UserProfileDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.StoryDomain.Models {
    public class Group : ObjectWithState, IEntity<int> {
        public int Id { get; set; }

        public int InterestId { get; set; }

        public string Title { get; set; }

        public Interest Interest { get; set; }

        public virtual ICollection<GroupMember> Members { get; set; }

        public virtual ICollection<Story> Stories { get; set; }
    }
}
