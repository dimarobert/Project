using Project.Account.Models;
using Project.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.UserProfileDomain.Models {
    public class UserGoal : ObjectWithState {
        public int Id { get; set; }

        public int GoalId { get; set; }

        [Required]
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }


        public virtual Goal Goal { get; set; }

        [ForeignKey("UserProfileId")]
        public virtual UserProfile UserProfile { get; set; }
    }
}
