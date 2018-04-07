using Project.Account.Models;
using Project.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.UserProfileDomain.Models {
    public class UserInterest : ObjectWithState {

        public int Id { get; set; }

        public int InterestId { get; set; }

        [Required]
        public string UserId { get; set; }



        public Interest Interest { get; set; }

        public UserInfo User { get; set; }

    }
}
