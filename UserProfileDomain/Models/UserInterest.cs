using Project.Account.Models;
using Project.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.UserProfileDomain.Models {
    public class UserInterest : ObjectWithState, IEntity<int> {

        public int Id { get; set; }

        public int InterestId { get; set; }

        [Required]
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }



        public virtual Interest Interest { get; set; }

        [ForeignKey("UserProfileId")]
        public virtual UserProfile UserProfile { get; set; }

    }
}
