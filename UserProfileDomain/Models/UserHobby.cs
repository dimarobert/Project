using Project.Account.Models;
using Project.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.UserProfileDomain.Models {
    public class UserGoal : ObjectWithState {
        public int Id { get; set; }

        public int HobbyId { get; set; }

        [Required]
        public string UserId { get; set; }


        public Goal Goal { get; set; }

        public UserInfo User { get; set; }
    }
}
