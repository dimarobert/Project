using Project.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.UserProfileDomain.Models {
    public class Goal : ObjectWithState {

        public int Id { get; set; }

        public string Title { get; set; }

        public int UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<Step> Steps { get; set; }

    }
}
